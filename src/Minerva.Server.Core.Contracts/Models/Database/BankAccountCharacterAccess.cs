namespace Minerva.Server.Core.Contracts.Models.Database
{
    public class BankAccountCharacterAccess
    {
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public bool CanWithdraw { get; set; }

        public bool CanSeeTransactions { get; set; }

        public bool CanManagePermissions { get; set; }

        /// <summary>
        /// If true, then this access can't be seen or changed by management view in game.
        /// </summary>
        public bool Hidden { get; set; }
    }
}