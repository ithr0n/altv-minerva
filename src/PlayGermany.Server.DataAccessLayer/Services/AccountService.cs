using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlayGermany.Server.DataAccessLayer.Services
{
    public class AccountService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public AccountService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task<bool> Exists(ulong socialClubId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            return dbContext.Accounts.AnyAsync(e => e.SocialClubId == socialClubId);
        }

        public async Task<Account> Authenticate(ulong socialClubId, ulong hardwareIdHash, ulong hardwareIdExHash, string password)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            using var alg = SHA512.Create();

            alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            var encrypted = BitConverter.ToString(alg.Hash);

            var account = await dbContext.Accounts.SingleOrDefaultAsync(e => 
                e.SocialClubId == socialClubId && 
                e.BannedUntil <= DateTime.Now &&
                e.Password == encrypted);

            if (account != null)
            {
                // check for faking authentication

                if (account.HardwareIdHash == null)
                {
                    account.HardwareIdHash = hardwareIdHash;
                    dbContext.Accounts.Update(account);
                }
                else if (account.HardwareIdHash != hardwareIdHash)
                {
                    account = null;
                }

                if (account.HardwareIdExHash == null)
                {
                    account.HardwareIdExHash = hardwareIdExHash;
                    dbContext.Accounts.Update(account);
                } else if (account.HardwareIdExHash != hardwareIdExHash)
                {
                    account = null;
                }

                if (account != null)
                {
                    await dbContext.SaveChangesAsync();
                }
            }

            return account;
        }
    }
}
