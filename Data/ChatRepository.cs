using AOL_Reborn.Models;

namespace AOL_Reborn.Data
{
    public class ChatRepository
    {
        private readonly AppDbContext _dbContex;
        // Save a new message to the database

        public ChatRepository(AppDbContext dbContext)
        {
            _dbContex = dbContext;
        }

        public ChatRepository()
        {
        }

        public void SaveMessage(ChatMessage message)
        {
            using (var db = new AppDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public User? GetUserByUserName(string username) => _dbContex.Users.FirstOrDefault(u => u.Username == username);

        // Load all messages between two users
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

        // Delete all messages between two users
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
        public int GetOrCreateConversationId(string user1, string user2)
        {
            using var db = new AppDbContext();

            // ✅ Check if a conversation already exists between these users
            var existingConversation = db.Conversations
                .FirstOrDefault(c =>
                    (c.ParticipantOne == user1 && c.ParticipantTwo == user2) ||
                    (c.ParticipantOne == user2 && c.ParticipantTwo == user1));

            if (existingConversation != null)
            {
                return existingConversation.Id; // ✅ Return existing conversation ID
            }

            // ✅ If no conversation exists, create a new one
            var newConversation = new Conversation
            {
                ParticipantOne = user1,
                ParticipantTwo = user2,
                CreatedAt = DateTime.UtcNow
            };

            db.Conversations.Add(newConversation);
            db.SaveChanges(); // ✅ Save the new conversation to the database

            return newConversation.Id;
        }


    }
}