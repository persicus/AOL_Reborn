using Microsoft.EntityFrameworkCore;
using AOL_Reborn.Models;

namespace AOL_Reborn.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=chatapp.db");
        }
    }
}
