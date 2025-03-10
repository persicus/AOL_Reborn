using System.ComponentModel.DataAnnotations;

namespace AOL_Reborn.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; } // Unique Conversation ID

        [Required]
        public required string ParticipantOne { get; set; } // First user in the chat

        [Required]
        public required string ParticipantTwo { get; set; } // Second user in the chat

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp when chat was created

        // Navigation property for related messages
        public List<ChatMessage> Messages { get; set; } = [];
    }
}