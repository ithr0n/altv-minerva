
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.DataAccessLayer.Models
{
    public class BankAccountAccess
    {
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public bool CanWithdraw { get; set; }

        public bool CanDeposit { get; set; }

        public bool CanSeeTransactions { get; set; }
    }
}