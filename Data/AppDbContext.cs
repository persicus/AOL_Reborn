using AOL_Reborn.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AOL_Reborn.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chatapp.db"); //Always use the same path
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

    }
}