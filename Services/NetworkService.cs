using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AOL_Reborn.Services
{
    public class NetworkService : NetworkManagerBase
    {
        private UdpClient _udpClient;
        private IPEndPoint _receiveEndPoint;
        private IPEndPoint _remoteEndPoint;

        public override async Task ConnectAsync(string server, int receivePort, int sendPort)
        {
            try
            {
                _udpClient = new UdpClient(receivePort);
                _receiveEndPoint = new IPEndPoint(IPAddress.Any, receivePort);
                _remoteEndPoint = new IPEndPoint(IPAddress.Parse(server), sendPort);

                StartListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting: {ex.Message}");
            }
        }

        private async void StartListening()
        {
            try
            {
                while (true)
                {
                    var result = await _udpClient.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                    OnMessageReceived(receivedMessage); // Notify UI
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UDP Receive Error: {ex.Message}");
            }
        }

        public override async Task SendMessageAsync(string message)
        {
            if (_udpClient != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await _udpClient.SendAsync(data, data.Length, _remoteEndPoint);
            }
        }
    }
}
