using Microsoft.EntityFrameworkCore;
using Minerva.Server.Contracts.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;
using Minerva.Server.DataAccessLayer.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class AccountService
        : ITranscientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public AccountService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> Exists(ulong socialClubId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var exists = await dbContext.Accounts.AnyAsync(e => e.SocialClubId == socialClubId);

            return exists;
        }

        public async Task<Account> Authenticate(ulong socialClubId, ulong hardwareIdHash, ulong hardwareIdExHash, string password)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            using var alg = SHA512.Create();

            alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            var builder = new StringBuilder();
            for (int i = 0; i < alg.Hash.Length; i++)
            {
                builder.Append(alg.Hash[i].ToString("x2"));
            }

            var encrypted = builder.ToString();

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
                }
                else if (account.HardwareIdExHash != hardwareIdExHash)
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
