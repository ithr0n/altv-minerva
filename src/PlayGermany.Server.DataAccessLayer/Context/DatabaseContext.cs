using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Models;

namespace PlayGermany.Server.DataAccessLayer.Context
{
    public class DatabaseContext
        : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        #region Entities

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<BankAccountAccess> BankAccountAccesses { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // combined primary key can only be configured here
            modelBuilder.Entity<BankAccountAccess>()
                .HasKey(c => new { c.BankAccountId, c.CharacterId });
        }
    }
}
