using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using wentylator.Pages;

public interface ISensorDataFetcher
{
    Task<SensorData> FetchSensorDataAsync();
}

namespace wentylator.ExtraClasses
{
    public class SensorDataFetcher : ISensorDataFetcher
    {
        private readonly string _arduinoIp = "192.168.188.106";
        private readonly int _port = 4567;

        public SensorDataFetcher()
        {
            _arduinoIp = FindArduinoIp();
        }

        private string FindArduinoIp()
        {
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                udpClient.Client.ReceiveTimeout = 5000;

                IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, _port);
                byte[] sendBuffer = Encoding.ASCII.GetBytes("FIND_ARDUINO");
                udpClient.Send(sendBuffer, sendBuffer.Length, broadcastEndpoint);

                try
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBuffer = udpClient.Receive(ref remoteEndPoint);
                    return remoteEndPoint.Address.ToString();
                }
                catch (SocketException ex)
                {
                    throw new Exception($"Failed to find Arduino IP address: {ex.Message}");
                }
            }
        }

        public async Task<SensorData> FetchSensorDataAsync()
        {
            try
            {
                using (UdpClient client = new UdpClient())
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_arduinoIp), _port);

                    string json = JsonSerializer.Serialize(new { command = "data" });
                    byte[] data = Encoding.UTF8.GetBytes(json);

                    await client.SendAsync(data, data.Length, endPoint);

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
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch sensor data: {ex.Message}");
            }
        }
    }

}
