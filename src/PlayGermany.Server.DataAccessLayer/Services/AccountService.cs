using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;
using System.Threading.Tasks;

namespace PlayGermany.Server.DataAccessLayer
{
    public class AccountService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public AccountService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        private DatabaseContext EnsureContextCreated(DatabaseContext context)
        {
            if (context == null)
            {
                context = _dbContextFactory.CreateDbContext();
            }

            return context;
        }

        public Task<Account> FindAccount(string socialClubId, DatabaseContext context = null)
        {
            EnsureContextCreated(ref context);

            return context.Accounts.FirstOrDefaultAsync(e => e.SocialClubId == socialClubId);
        }

        public async bool Authenticate(string socialClubId, string password, DatabaseContext context = null)
        {
            using var context = EnsureContextCreated(ref context);

            var account = await dbContext.Accounts.FirstOrDefaultAsync(e => e.SocialClubId == socialClubId);
        }
    }
}
