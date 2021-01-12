using AltV.Net.EntitySync;
using Minerva.Server.Enums;
using System.Numerics;

namespace Minerva.Server.EntitySync.Entities
{
    public class StaticBlip
        : Entity
    {
        public StaticBlip(Vector3 position, int dimension, uint range)
            : base((ulong)EntitySyncType.StaticBlip, position, dimension, range)
        {
        }

        /// <summary>
        /// The text to display on the blip in the map menu
        /// </summary>
        public string Name
        {
            get => TryGetData("name", out string name) ? name : string.Empty;
            set { if (Name != value) SetData("name", value); }
        }

        /// <summary>
        /// ID of the sprite to use, can be found on the ALTV wiki
        /// </summary>
        public int Sprite
        {
            get => TryGetData("sprite", out int sprite) ? sprite : 0;
            set { if (Sprite != value) SetData("sprite", value); }
        }

        /// <summary>
        /// Blip Color code, can also be found on the ALTV wiki
        /// </summary>
        public int Color
        {
            get => TryGetData("color", out int color) ? color : 0;
            set { if (Color != value) SetData("color", value); }
        }

        /// <summary>
        /// Scale of the blip, 1 is regular size.
        /// </summary>
        public float Scale
        {
            get => TryGetData("scale", out float scale) ? scale : 1;
            set { if (Scale != value) SetData("scale", value); }
        }

        /// <summary>
        /// Whether this blip can be seen on the minimap from anywhere on the map, or only when close to it(it will always show on the main map).
        /// </summary>
        public bool ShortRange
        {
            get => TryGetData("shortRange", out bool shortRange) ? shortRange : true;
            set { if (ShortRange != value) SetData("shortRange", value); }
        }
    }
}
