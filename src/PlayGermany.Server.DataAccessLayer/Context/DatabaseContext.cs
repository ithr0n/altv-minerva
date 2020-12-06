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

        #endregion
    }
}
