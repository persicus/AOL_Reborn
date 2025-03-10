using AOL_Reborn.Services;

public class NetworkChatService : IChatService
{
    private static readonly Lazy<NetworkChatService> _instance = new Lazy<NetworkChatService>(() => new NetworkChatService());
    public static NetworkChatService Instance => _instance.Value;

    private readonly NetworkService _networkService;

    public event Action<string> MessageReceived = delegate { };

    // Private constructor ensures no external instantiation
    private NetworkChatService()
    {
        _networkService = new NetworkService();
        _networkService.MessageReceived += message => MessageReceived?.Invoke(message);
    }

    public async Task ConnectAsync(string server, int receivePort, int sendPort)
    {
        await _networkService.ConnectAsync(server, receivePort, sendPort);
    }

    public async Task SendMessageAsync(string message)
    {
        await _networkService.SendMessageAsync(message);
    }
}