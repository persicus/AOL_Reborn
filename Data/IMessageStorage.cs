using AOL_Reborn.Models;

public interface IMessageStorage
{
    void SaveMessage(ChatMessage message);

    Task SaveMessageAsync(ChatMessage message);

    List<ChatMessage> GetMessages(string sender, string receiver);

    void DeleteChatHistory(string sender, string receiver);

    // New method to ensure conversations exist
    int GetOrCreateConversationId(string userOne, string userTwo);
}