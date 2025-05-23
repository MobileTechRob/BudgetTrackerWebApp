using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyCostTracker
{
    public class AppDbContext : DbContext
    {
        public DbSet<DailyTransaction> DailyTransactions { get; set; }
        public DbSet<KeywordToCostCategory> KeywordToCostCategory { get; set; }
        public DbSet<KeywordToSavingsCategory> KeywordToSavingsCategory { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ROBSPC\\SQLEXPRESS;Database=DailyCostTracker;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyTransaction>().HasKey(d => d.Content);
            modelBuilder.Entity<KeywordToCostCategory>().HasNoKey();
            modelBuilder.Entity<KeywordToSavingsCategory>().HasNoKey();
        }
    }
}
