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
            var conversation = db.Conversations
                .FirstOrDefault(c =>
                    (c.ParticipantOne == sender && c.ParticipantTwo == receiver) ||
                    (c.ParticipantOne == receiver && c.ParticipantTwo == sender));

            if (conversation == null)
                return new List<ChatMessage>();

            return db.Messages
                .Where(m => m.ConversationId == conversation.Id)
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

    public int GetOrCreateConversationId(string userOne, string userTwo)
    {
        using (var db = new AppDbContext())
        {
            var conversation = db.Conversations
                .FirstOrDefault(c =>
                    (c.ParticipantOne == userOne && c.ParticipantTwo == userTwo) ||
                    (c.ParticipantOne == userTwo && c.ParticipantTwo == userOne));

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    ParticipantOne = userOne,
                    ParticipantTwo = userTwo,
                    CreatedAt = DateTime.Now
                };
                db.Conversations.Add(conversation);
                db.SaveChanges();
            }

            return conversation.Id;
        }
    }
}
