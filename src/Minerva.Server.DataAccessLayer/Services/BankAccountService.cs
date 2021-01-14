using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;
using Minerva.Server.DataAccessLayer.Models;

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

        public async Task<BankAccount> NewBankAccountForCharacter(Character owner)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var bankAcc = new BankAccount();

            dbContext.BankAccounts.Add(bankAcc);

            var bankAccAccess = new BankAccountAccess
            {
                BankAccountId = bankAcc.Id,
                CharacterId = owner.Id,
                CanDeposit = true,
                CanWithdraw = true,
                CanSeeTransactions = true
            };

            dbContext.BankAccountAccesses.Add(bankAccAccess);

            await dbContext.SaveChangesAsync();

            return bankAcc;
        }
    }
}