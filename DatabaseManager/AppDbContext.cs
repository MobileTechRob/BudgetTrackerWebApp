using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.DataModels;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace DatabaseManager
{
    public class AppDbContext : DbContext
    {
        public DbSet<DailyTransaction> DailyTransactions { get; set; }
        public DbSet<KeywordToCostCategory> KeywordToCostCategory { get; set; }
        public DbSet<KeywordToSavingsCategory> KeywordToSavingsCategory { get; set; }

        string ConnectionString = "";

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
              : base(dbContextOptions)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyTransaction>().HasKey(d => d.Content);
            modelBuilder.Entity<KeywordToCostCategory>().HasNoKey();
            modelBuilder.Entity<KeywordToSavingsCategory>().HasNoKey();
        }
    }
}
