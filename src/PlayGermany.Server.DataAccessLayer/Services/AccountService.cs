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

            return await dbContext.Accounts.SingleAsync(e => 
                e.SocialClubId == socialClubId && 
                e.HardwareIdHash == hardwareIdHash &&
                e.HardwareIdExHash == hardwareIdExHash &&
                e.BannedUntil <= DateTime.Now &&
                e.Password == encrypted);
        }
    }
}
