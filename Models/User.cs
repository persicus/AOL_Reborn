using System.ComponentModel.DataAnnotations;

namespace AOL_Reborn.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } // Unique User ID

        [Required]
        public required string Username { get; set; } // Unique identifier for the user

        public required string DisplayName { get; set; } // Optional: User-friendly display name

        public bool IsOnline { get; set; } = false; // Tracks online status
    }
}