using AOL_Reborn.Models;
using System.Linq;
using System.Collections.Generic;

namespace AOL_Reborn.Data
{
    public class ChatRepository
    {
        private readonly AppDbContext _dbContext;

        public ChatRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveMessage(ChatMessage message)
        {
            using (var db = new AppDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public User? GetUserByUserName(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username);
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

        // Retrieves an existing conversation between two users, or creates one if it doesn't exist.
        public int GetOrCreateConversationId(string user1, string user2)
        {
            using var db = new AppDbContext();

            var existingConversation = db.Conversations
                .FirstOrDefault(c =>
                    (c.ParticipantOne == user1 && c.ParticipantTwo == user2) ||
                    (c.ParticipantOne == user2 && c.ParticipantTwo == user1));

            if (existingConversation != null)
            {
                return existingConversation.Id;
            }

            var newConversation = new Conversation
            {
                ParticipantOne = user1,
                ParticipantTwo = user2,
                CreatedAt = DateTime.UtcNow
            };

            db.Conversations.Add(newConversation);
            db.SaveChanges();

            return newConversation.Id;
        }
    }
}
