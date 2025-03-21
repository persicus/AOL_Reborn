﻿using AOL_Reborn.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AOL_Reborn.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chatapp.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
