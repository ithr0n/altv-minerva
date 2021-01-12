using AltV.Net.EntitySync;
using Minerva.Server.Enums;
using System.Numerics;

namespace Minerva.Server.EntitySync.Entities
{
    public class Ped
        : Entity
    {
        // https://pastebin.com/s6bmd45N
        public Ped(Vector3 position, int dimension, uint range)
            : base((ulong) EntitySyncType.Marker, position, dimension, range)
        {
        }
    }
}
