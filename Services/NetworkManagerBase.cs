namespace AOL_Reborn.Services
{
    public abstract class NetworkManagerBase
    {
        public abstract Task ConnectAsync(string server, int receivePort, int sendPort);
        public abstract Task SendMessageAsync(string message);
        public event Action<string> MessageReceived;

        protected void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }
    }
}
