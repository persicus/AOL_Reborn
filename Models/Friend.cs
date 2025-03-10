using System.ComponentModel.DataAnnotations;

namespace AOL_Reborn.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsOnline { get; set; }
    }
}
