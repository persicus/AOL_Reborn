using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AOL_Reborn.Services
{
    public class NetworkService : NetworkManagerBase
    {
        private UdpClient? _udpClient;

        private IPEndPoint? _remoteEndPoint;

        public event Action<string, bool> StatusUpdated = delegate { };

        public NetworkService()
        {
            _udpClient = new UdpClient();
        }

        public override async Task ConnectAsync(string server, int receivePort, int sendPort)
        {
            try
            {
                Disconnect(); // Ensure old connection is closed

                // Create a new UdpClient instance
                _udpClient = new UdpClient();

                // Allow socket address reuse
                _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                // Disable exclusive address use
                _udpClient.Client.ExclusiveAddressUse = false;

                // Bind the client to the local receive port
                _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));

                // Set up the remote endpoint using the provided server and sendPort
                _remoteEndPoint = new IPEndPoint(IPAddress.Parse(server), sendPort);

                // Begin asynchronous listening for incoming messages
                await StartListening();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting: {ex.Message}");
            }
        }



        public void Disconnect()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null;
            }
        }

        private async Task StartListening()
        {
            if (_udpClient == null)
            {
                Console.WriteLine("UDP Client is null, stopping listener.");
                return;
            }

            try
            {
                while (_udpClient != null) // Prevents listening if disconnected
                {
                    var result = await _udpClient.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(result.Buffer);

                    if (!string.IsNullOrEmpty(receivedMessage))
                    {
                        OnMessageReceived(receivedMessage);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("UDP Client was disposed, stopping listener.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UDP receiving: {ex.Message}");
            }
        }

        public void SendOfflineStatus()
        {
            if (_udpClient == null)
            {
                Console.WriteLine("Warning: Attempted to send offline status, but UDP client was null.");
                return;
            }

            string offlineMessage = $"{Environment.UserName}|Offline";
            byte[] data = Encoding.UTF8.GetBytes(offlineMessage);
            _udpClient.SendAsync(data, data.Length, _remoteEndPoint);
        }

        public override async Task SendMessageAsync(string message)
        {
            if (_udpClient == null)
            {
                Console.WriteLine("Warning: Attempted to send a message, but UDP client was null.");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(message);
            await _udpClient.SendAsync(data, data.Length, _remoteEndPoint);
        }
    }
}