using System.Net.Sockets;
using System.Net;
using System.Text;

namespace wentylator.ExtraClasses
{
    public class ArduinoIPFinder
    {
        private UdpClient udpClient;

        public ArduinoIPFinder()
        {
            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
        }

        public void SendBroadcast(string message)
        {
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, 4567);
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            udpClient.Send(sendBuffer, sendBuffer.Length, broadcastEndpoint);
        }

        public IPAddress ReceiveResponse(int timeout)
        {
            Console.WriteLine("Broadcast message sent. Waiting for response...");

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            udpClient.Client.ReceiveTimeout = timeout;

            try
            {
                byte[] receiveBuffer = udpClient.Receive(ref remoteEndPoint);
                if (receiveBuffer.Length == 4)
                {
                    IPAddress arduinoIp = new IPAddress(receiveBuffer);
                    Console.WriteLine($"Received response from Arduino at IP: {arduinoIp}");
                    return arduinoIp;
                }
                else
                {
                    Console.WriteLine("Received a response, but it's not a valid IP address.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"No response received from Arduino. Error: {ex.Message}");
            }

            return null;
        }

        public void Close()
        {
            udpClient.Close();
        }
    }
}
