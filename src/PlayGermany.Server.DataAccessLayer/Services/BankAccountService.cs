using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;

namespace PlayGermany.Server.DataAccessLayer.Services
{
    public class BankAccountService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public BankAccountService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public BankAccount NewBankAccountForCharacter(Character owner)
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

            dbContext.SaveChanges();

            return bankAcc;
        }
    }
}