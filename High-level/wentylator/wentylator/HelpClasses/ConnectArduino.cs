using System.Net.Sockets;
using System.Net;
using System.Text;

namespace CoolFan.HelpClasses
{
    public class ConnectArduino
    {
        public string _arduinoIp = "192.168.188.108";
        private int _port = 4567;
        private int _receivePort = 1928;

        string command = "initialize";

        public ConnectArduino()
        {
            establishconnection();
        }

        public async void establishconnection()
        {
            using (UdpClient client = new UdpClient(_receivePort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_arduinoIp), _port);

                byte[] data = Encoding.UTF8.GetBytes(command);

                await client.SendAsync(data, data.Length, endPoint);

                client.Client.ReceiveTimeout = 5000;

                try
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string response = Encoding.UTF8.GetString(result.Buffer);
                    Console.WriteLine($"Received response: {response}");
                    _arduinoIp = response;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving response: {ex.Message}");
                }
            }
        }
    }
}
