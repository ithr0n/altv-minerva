using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.Contracts.Models;
using Minerva.Server.Core.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class BankAccountService
        : ITransientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public BankAccountService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<BankAccount> CreateNewAccountForCharacter(Character owner)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var bankAcc = new BankAccount();

            dbContext.BankAccounts.Add(bankAcc);

            var bankAccAccess = new BankAccountCharacterAccess
            {
                BankAccountId = bankAcc.Id,
                CharacterId = owner.Id,
                CanWithdraw = true,
                CanSeeTransactions = true,
                CanManagePermissions = true
            };

            dbContext.BankAccountCharacterAccesses.Add(bankAccAccess);

            await dbContext.SaveChangesAsync();

            return bankAcc;
        }

        public async Task<BankAccount> CreateNewAccountForGroup(Group group)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var bankAcc = new BankAccount();

            dbContext.BankAccounts.Add(bankAcc);

            var bankAccAccess = new BankAccountGroupAccess
            {
                BankAccountId = bankAcc.Id,
                GroupId = group.Id,
                WithdrawLevel = 0,
                SeeTransactionsLevel = 0,
                ManagePermissionsLevel = 0
            };

            dbContext.BankAccountGroupAccesses.Add(bankAccAccess);

            await dbContext.SaveChangesAsync();

            return bankAcc;
        }

        public async Task GrantFullAccessToAccount(Character character, BankAccount bankAccount)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var access = new BankAccountCharacterAccess
            {
                BankAccountId = bankAccount.Id,
                CharacterId = character.Id,
                CanSeeTransactions = true,
                CanWithdraw = true
            };

            dbContext.BankAccountCharacterAccesses.Update(access);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<BankAccountCharacterAccess>> GetCharacterAccessesOfAccount(BankAccount bankAccount)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var result = dbContext.BankAccountCharacterAccesses.Where(e => e.BankAccountId == bankAccount.Id && !e.Hidden);

            return await result.ToListAsync();
        }

        public async Task<List<BankAccountGroupAccess>> GetGroupAccessesOfAccount(BankAccount bankAccount)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var result = dbContext.BankAccountGroupAccesses.Where(e => e.BankAccountId == bankAccount.Id && !e.Hidden);

            return await result.ToListAsync();
        }

        public async Task DenyAccessToAccount(Character character, BankAccount bankAccount)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var accessToRemove = dbContext.BankAccountCharacterAccesses.FirstOrDefault(e => e.BankAccountId == bankAccount.Id && e.CharacterId == character.Id);

            if (accessToRemove != null)
            {
                dbContext.BankAccountCharacterAccesses.Remove(accessToRemove);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}