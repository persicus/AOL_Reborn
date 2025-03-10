using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AOL_Reborn.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Foreign Key to Conversation
        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }

        public Conversation? Conversation { get; set; }

        //Private constructor to enforce factory usage
        private ChatMessage(string sender, string receiver, string message, int conversationId)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            Timestamp = DateTime.Now;
            ConversationId = conversationId;
        }

        // Factory Method to ensure valid object creation
        public static ChatMessage Create(string sender, string receiver, string message, int conversationId)
        {
            if (string.IsNullOrWhiteSpace(sender) || string.IsNullOrWhiteSpace(receiver) || string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Sender, receiver, and message cannot be empty.");

            return new ChatMessage(sender, receiver, message, conversationId);
        }
    }
}