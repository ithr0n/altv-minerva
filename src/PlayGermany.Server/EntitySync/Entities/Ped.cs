using AltV.Net.EntitySync;
using PlayGermany.Server.Enums;
using System.Numerics;

namespace PlayGermany.Server.EntitySync.Entities
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
