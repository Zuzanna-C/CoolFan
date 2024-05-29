using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CoolFan.HelpClasses
{
    public class ArduinoDiscoverer
    {
        private int port;
        private UdpClient udpClient;
        public IPAddress arduinoIpAddress;

        public event Action<string> IpAddressDiscovered;

        public ArduinoDiscoverer(int port)
        {
            this.port = port;
            this.udpClient = new UdpClient();
            this.udpClient.EnableBroadcast = true;
        }


        public async Task DiscoverArduinoAsync()
        {
            string discoverMessage = "{\"command\": \"DISCOVER\"}";
            byte[] discoverBytes = Encoding.ASCII.GetBytes(discoverMessage);
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, port);

            udpClient.Send(discoverBytes, discoverBytes.Length, broadcastEndPoint);

            while (true)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string message = Encoding.ASCII.GetString(result.Buffer);

                    if (message.StartsWith("IP:"))
                    {
                        string ipAddress = message.Substring(3);
                        arduinoIpAddress = IPAddress.Parse(ipAddress);
                        IpAddressDiscovered?.Invoke(ipAddress);
                        break; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving UDP packet: {ex.Message}");
                }
            }
        }

        public void StopDiscovery()
        {
            udpClient.Close();
        }

        public async Task SendCommandAsync(string command)
        {
            if (arduinoIpAddress == null)
            {
                Console.WriteLine("Arduino IP address not discovered yet.");
                return;
            }

            var commandObject = new JObject { ["command"] = command };
            string commandMessage = commandObject.ToString();
            byte[] commandBytes = Encoding.ASCII.GetBytes(commandMessage);
            IPEndPoint arduinoEndPoint = new IPEndPoint(arduinoIpAddress, port);

            try
            {
                await udpClient.SendAsync(commandBytes, commandBytes.Length, arduinoEndPoint);
                Console.WriteLine($"Sent command: {command}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending UDP packet: {ex.Message}");
            }
        }
    }
}
