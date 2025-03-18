using AOL_Reborn.Audio;
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
        _networkService.MessageReceived += message =>
        {
            // Play "message received" sound
            AudioManager.PlaySound("Voicy_Instant Message");

            MessageReceived?.Invoke(message);
        };
    }

    public async Task ConnectAsync(string server, int receivePort, int sendPort)
    {
        await _networkService.ConnectAsync(server, receivePort, sendPort);
    }

    public async Task SendMessageAsync(string message)
    {
        // Play "message send" sound
        AudioManager.PlaySound("Voicy_Drop.mp3");

        await _networkService.SendMessageAsync(message);
    }



}