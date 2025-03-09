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

        public event Action<string, bool> StatusUpdated;


        public override async Task ConnectAsync(string server, int receivePort, int sendPort)
        {
            try
            {
                if (_udpClient == null) //Ensure _udpClient is initialized
                {
                    _udpClient = new UdpClient(receivePort);
                }

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

                    if (!string.IsNullOrEmpty(receivedMessage))
                    {
                        OnMessageReceived(receivedMessage); // Properly invoke the base class event
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UDP Receive Error: {ex.Message}");
            }
        }



        public void SendOfflineStatus()
        {
            if (_udpClient == null) //Prevent sending if _udpClient is null
            {
                Console.WriteLine("Warning: Attempted to send offline status, but UDP client was null.");
                return;
            }

            string offlineMessage = $"{Environment.UserName}|Offline";
            byte[] data = Encoding.UTF8.GetBytes(offlineMessage);
            _udpClient.SendAsync(data, data.Length, _remoteEndPoint);
        }
        public void Disconnect()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null; // Ensure it's null after disconnecting
            }
        }




        public override async Task SendMessageAsync(string message)
        {
            if (_udpClient == null)
            {
                Console.WriteLine("Warning: Attempted to send a message, but UDP client was null.");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message);
            await _udpClient.SendAsync(data, data.Length, _remoteEndPoint); // Ensure the right port is used
        }

    }
}
