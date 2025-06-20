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
        public DbSet<ReceiptsFromCashTransactions> ReceiptsFromCashTransactions { get; set; }
        public DbSet<BudgetInterval> BudgetIntervals { get; set; }        
        public DbSet<ImportTransactionDataLog> ImportTransactionDataLog { get; set; }  

        string connectionString = "";

        public AppDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
              : base(dbContextOptions)
        {                     
        }

        // Override OnConfiguring method to set the connection string dynamically
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlServer(this.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyTransaction>().HasKey(d => d.Fi_Transaction_Reference);
            modelBuilder.Entity<DailyTransaction>(entity => { entity.Property(p => p.Amount).HasColumnType("decimal(15, 2)"); });
            modelBuilder.Entity<DailyTransaction>(entity => { entity.Property(p => p.Original_Amount).HasColumnType("decimal(15, 2)"); });
            modelBuilder.Entity<KeywordToCostCategory>().HasKey(d => d.keyword);
            modelBuilder.Entity<KeywordToSavingsCategory>().HasKey(d => d.keyword);
            modelBuilder.Entity<ReceiptsFromCashTransactions>().HasKey(d => d.posted_date);
            modelBuilder.Entity<BudgetInterval>().HasKey(table => table.IntervalName);
            modelBuilder.Entity<ImportTransactionDataLog>().HasKey(d => d.DateTimeOfImport);  
        }
    }
}
