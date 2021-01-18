using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Minerva.Server.Core.Contracts.Models.Base;

namespace Minerva.Server.Core.Contracts.Models.Database
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
