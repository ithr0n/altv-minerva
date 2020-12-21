
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            Accesses = new List<BankAccountAccess>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal Value { get; set; }

        public List<BankAccountAccess> Accesses { get; set; }
    }
}