
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;

public interface IFanControlService
{
    Task SendCommandAsync(string command);
}

namespace wentylator.HelpClasses
{
    public class FanControlService : IFanControlService
    {
        private readonly string _arduinoIp = "192.168.188.106";
        private readonly int _port = 4567;

        public async Task SendCommandAsync(string command)
        {
            using (UdpClient client = new UdpClient())
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_arduinoIp), _port);

                string json = JsonSerializer.Serialize(new { command });
                byte[] data = Encoding.UTF8.GetBytes(json);

                await client.SendAsync(data, data.Length, endPoint);
            }
        }
    }
}
