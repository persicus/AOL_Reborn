using System;
using System.Threading.Tasks;
using AOL_Reborn.Services;

public class NetworkChatService : IChatService
{
    private readonly NetworkService _networkService;

    public event Action<string> MessageReceived;

    public NetworkChatService()
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
