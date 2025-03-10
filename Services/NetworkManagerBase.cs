namespace AOL_Reborn.Services
{
    public abstract class NetworkManagerBase
    {
        public event Action<string> MessageReceived = delegate { };

        protected void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        public abstract Task ConnectAsync(string server, int receivePort, int sendPort);

        public abstract Task SendMessageAsync(string message);
    }
}