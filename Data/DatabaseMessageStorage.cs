using AOL_Reborn.Data;
using AOL_Reborn.Models;

public class DatabaseMessageStorage : IMessageStorage
{
    public void SaveMessage(ChatMessage message)
    {
        using (var db = new AppDbContext())
        {
            db.Messages.Add(message);
            db.SaveChanges();
        }
    }

    public async Task SaveMessageAsync(ChatMessage message)
    {
        await Task.Run(() =>
        {
            using (var db = new AppDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        });
    }

    public List<ChatMessage> GetMessages(string sender, string receiver)
    {
        using (var db = new AppDbContext())
        {
            return db.Messages
                .Where(m => (m.Sender == sender && m.Receiver == receiver) ||
                            (m.Sender == receiver && m.Receiver == sender))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }
    }

    public void DeleteChatHistory(string sender, string receiver)
    {
        using (var db = new AppDbContext())
        {
            var messages = db.Messages
                .Where(m => (m.Sender == sender && m.Receiver == receiver) ||
                            (m.Sender == receiver && m.Receiver == sender))
                .ToList();

            db.Messages.RemoveRange(messages);
            db.SaveChanges();
        }
    }
}
