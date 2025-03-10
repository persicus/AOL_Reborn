using System;
using System.Collections.Generic;
using System.Linq;
using AOL_Reborn.Models;
using Microsoft.EntityFrameworkCore;

namespace AOL_Reborn.Data
{
    public class ChatRepository
    {
        // Save a new message to the database
        public void SaveMessage(ChatMessage message)
        {
            using (var db = new AppDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

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
    }
}
