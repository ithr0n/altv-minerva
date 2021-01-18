using System.Numerics;

namespace Minerva.Server.Data.Models
{
    public class WorldAtmObject
    {
        public string Name { get; set; }
        public uint Hash { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}
