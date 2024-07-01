using System.Net.Sockets;
using System.Net;
using System.Text;
using CoolFan.Models;
using CoolFan.Interfaces;
using System.Globalization;

namespace CoolFan.HelpClasses
{
    public class SensorDataFetcher : ISensorDataFetcher
    {
        public ConnectArduino _connectArduino; // = new ConnectArduino();
        private readonly string _arduinoIp;
        private static int _port = 4567;
        private static int _receivePort = 1928;

        UdpClient client = new UdpClient(_receivePort);

        string command = "data";

        private SensorData _sensorData = new SensorData();

        public SensorDataFetcher() 
        {
            //_arduinoIp = _connectArduino._arduinoIp;
        }

        public async Task<SensorData> getSensorDataAsync()
        {
            await fetchData();
            return _sensorData;
        }

        public async Task fetchData()
        {
            using (client)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.188.108"), _port); //ten adres nie ma być statyczny

                byte[] data = Encoding.UTF8.GetBytes(command);

                await client.SendAsync(data, data.Length, endPoint);

                client.Client.ReceiveTimeout = 5000;

                try
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string response = Encoding.UTF8.GetString(result.Buffer);

                    if (command == "data")
                    {
                        string[] parts = response.Split(',');

                        if (parts.Length == 2)
                        {
                            float temperature = float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat);
                            float humidity = float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat);
                            _sensorData = new SensorData
                            {
                                Temperature = temperature,
                                Humidity = humidity
                            };
                            Console.WriteLine($"Temperature: {_sensorData.Temperature}, Humidity: {_sensorData.Humidity}");
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse sensor data.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Received response: {response}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving response: {ex.Message}");
                }
            }
        }
    }
}
