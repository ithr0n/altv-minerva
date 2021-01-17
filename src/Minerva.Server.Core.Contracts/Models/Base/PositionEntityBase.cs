using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Minerva.Server.Core.Contracts.Models.Base
{
    public abstract class PositionEntityBase
    {
        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        [NotMapped]
        public Vector3 Position
        {
            get => new Vector3(PositionX, PositionY, PositionZ);
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
                PositionZ = value.Z;
            }
        }
    }
}
