using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlayGermany.Server.DataAccessLayer.Models.Base;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class GameDoor
        : PositionRotationEntityBase, ILockableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Locked { get; set; }

        public Guid KeyDataId { get; set; }
        public KeyData KeyData { get; set; }
    }
}
