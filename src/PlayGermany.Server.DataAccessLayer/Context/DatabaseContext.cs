using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Models;

namespace PlayGermany.Server.DataAccessLayer.Context
{
    public class DatabaseContext
        : DbContext
    {
        private readonly string _databaseConnectionString;

        public DatabaseContext(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_databaseConnectionString, new MariaDbServerVersion("10.5.6"));
        }

        #region Entities

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Character> Characters { get; set; }

        #endregion
    }
}
