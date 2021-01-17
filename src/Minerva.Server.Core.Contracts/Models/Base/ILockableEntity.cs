using System;

namespace Minerva.Server.Core.Contracts.Models.Base
{
    public interface ILockableEntity
    {
        public bool Locked { get; set; }

        public Guid KeyDataId { get; set; }
        public KeyData KeyData { get; set; }
    }
}
