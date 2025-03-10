using AOL_Reborn.Models;

public interface IMessageStorage
{
    void SaveMessage(ChatMessage message);
    Task SaveMessageAsync(ChatMessage message); // Ensure async version exists
    List<ChatMessage> GetMessages(string sender, string receiver);
    void DeleteChatHistory(string sender, string receiver);
}