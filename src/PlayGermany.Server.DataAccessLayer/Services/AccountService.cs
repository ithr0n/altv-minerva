using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;
using System;
using System.Security.Cryptography;
using System.Text;
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

        public Task<Account> FindAccount(string socialClubId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            return dbContext.Accounts.FirstOrDefaultAsync(e => e.SocialClubId == socialClubId);
        }

        public Task<bool> Authenticate(string socialClubId, string password)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            using var alg = SHA512.Create();

            alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            var encrypted = BitConverter.ToString(alg.Hash);

            return dbContext.Accounts.AnyAsync(e => e.SocialClubId == socialClubId && e.Password == encrypted);
        }
    }
}
