using PlayGermany.Server.DataAccessLayer.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class Character
        : PositionRotationEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ushort Health { get; set; }

        public ushort Armor { get; set; }

        public decimal Cash { get; set; }

        [NotMapped]
        public string Name => $"{FirstName} {LastName}";
    }
}
