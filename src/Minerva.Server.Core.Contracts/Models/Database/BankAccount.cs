using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.Core.Contracts.Models.Database
{
    public class BankAccount
    {
        public BankAccount()
        {
            CharacterAccesses = new List<BankAccountCharacterAccess>();
            GroupAccesses = new List<BankAccountGroupAccess>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal Value { get; set; }

        public List<BankAccountCharacterAccess> CharacterAccesses { get; set; }

        public List<BankAccountGroupAccess> GroupAccesses { get; set; }
    }
}