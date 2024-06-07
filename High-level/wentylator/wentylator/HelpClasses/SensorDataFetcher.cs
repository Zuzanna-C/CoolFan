using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using CoolFan.Models;
using CoolFan.Interfaces;

namespace CoolFan.HelpClasses
{
    public class SensorDataFetcher : ISensorDataFetcher
    {
        private readonly string _arduinoIp;
        private readonly int _port = 4567;

        public SensorDataFetcher(string arduinoIP) 
        {
            _arduinoIp = arduinoIP;
        }

        public async Task<SensorData> FetchSensorDataAsync()
        {
            using (UdpClient client = new UdpClient(_port))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_arduinoIp), _port);

                string json = JsonSerializer.Serialize(new { command = "data" });
                byte[] data = Encoding.UTF8.GetBytes(json);

                await client.SendAsync(data, data.Length, endPoint);

                client.Client.ReceiveTimeout = 5000;

                UdpReceiveResult result = await client.ReceiveAsync();
                string jsonResponse = Encoding.UTF8.GetString(result.Buffer);

                var sensorDataDict = JsonSerializer.Deserialize<Dictionary<string, float>>(jsonResponse);

                if (sensorDataDict != null &&
                    sensorDataDict.TryGetValue("temperature", out float temperature) &&
                    sensorDataDict.TryGetValue("humidity", out float humidity))
                {
                    return new SensorData
                    {
                        Temperature = temperature,
                        Humidity = humidity
                    };
                }
                else
                {
                    throw new Exception("Failed to deserialize sensor data.");
                }
            }
        }
    }
}
