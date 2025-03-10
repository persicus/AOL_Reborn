using System.ComponentModel.DataAnnotations;

namespace AOL_Reborn.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ParticipantOne { get; set; }  // Represents one user in the chat

        [Required]
        public string ParticipantTwo { get; set; }  // Represents the other user in the chat

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property for the messages in this conversation
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
