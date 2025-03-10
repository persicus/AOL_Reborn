public interface IChatService
{
    event Action<string> MessageReceived;
    Task ConnectAsync(string server, int receivePort, int sendPort);
    Task SendMessageAsync(string message);
}
