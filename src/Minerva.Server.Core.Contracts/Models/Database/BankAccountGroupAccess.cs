namespace Minerva.Server.Core.Contracts.Models.Database
{
    public class BankAccountGroupAccess
    {
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public uint WithdrawLevel { get; set; }

        public uint SeeTransactionsLevel { get; set; }

        public uint ManagePermissionsLevel { get; set; }

        /// <summary>
        /// If true, then this access can't be seen or changed by management view in game.
        /// </summary>
        public bool Hidden { get; set; }
    }
}