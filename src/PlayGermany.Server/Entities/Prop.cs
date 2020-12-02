using AltV.Net.EntitySync;
using PlayGermany.Server.EntitySync.WritableData;
using PlayGermany.Server.Enums;
using System.Numerics;

namespace PlayGermany.Server.Entities
{
    public class Prop
        : Entity
    {
        public Prop(Vector3 position, int dimension, uint range)
            : base((ulong)EntitySyncType.Prop, position, dimension, range)
        {
        }

        /// <summary>
        /// Set or get the current object's model.
        /// </summary>
        public uint Model
        {
            get => TryGetData("model", out uint model) ? model : default;
            set { if (Model != value) SetData("model", value); }
        }

        /// <summary>
        /// Set or get the current object's rotation (in degrees).
        /// </summary>
        public WritableVector3 Rotation
        {
            get => TryGetData("rotation", out WritableVector3 rotation) ? rotation : null;
            set { if (Rotation != value) SetData("rotation", value); }
        }

        /// <summary>
        /// Set or get LOD Distance of the object.
        /// </summary>
        public uint? LodDistance
        {
            get => TryGetData("lodDistance", out uint lodDistance) ? lodDistance : null;
            set { if (LodDistance != value) SetData("lodDistance", value); }
        }

        /// <summary>
        /// Get or set the current texture variation, use null to reset it to default.
        /// </summary>
        public TextureVariation? TextureVariation
        {
            get => TryGetData("textureVariation", out TextureVariation textureVariation) ? textureVariation : null;
            set { if (TextureVariation != value) SetData("textureVariation", value); }
        }

        /// <summary>
        /// Get or set the object's dynamic state. Some objects can be moved around by the player when dynamic is set to true.
        /// </summary>
        public bool? Dynamic
        {
            get => TryGetData("dynamic", out bool dynamic) ? dynamic : null;
            set { if (Dynamic != value) SetData("dynamic", value); }
        }

        /// <summary>
        /// Set/get visibility state of object
        /// </summary>
        public bool? Visible
        {
            get => TryGetData("visible", out bool visible) ? visible : null;
            set { if (Visible != value) SetData("visible", value); }
        }

        /// <summary>
        /// Set/get an object on fire, NOTE: does not work very well as of right now, fire is very small.
        /// </summary>
        public bool? OnFire
        {
            get => TryGetData("onFire", out bool onFire) ? onFire : null;
            set { if (OnFire != value) SetData("onFire", value); }
        }

        /// <summary>
        /// Freeze an object into it's current position. or get it's status
        /// </summary>
        public bool? Freezed
        {
            get => TryGetData("freezed", out bool freezed) ? freezed : null;
            set { if (Freezed != value) SetData("freezed", value); }
        }

        /// <summary>
        /// Set the light color of the object, use null to reset it to default.
        /// </summary>
        public WritableRgb LightColor
        {
            get => TryGetData("lightColor", out WritableRgb lightColor) ? lightColor : default;
            set { if (LightColor != value) SetData("lightColor", value); }
        }

        public WritableVector3 Velocity
        {
            get => TryGetData("velocity", out WritableVector3 velocity) ? velocity : default;
            set { if (Velocity != value) SetData("velocity", value); }
        }

        public WritableVector3 SlideToPosition
        {
            get => TryGetData("slideToPosition", out WritableVector3 slideToPosition) ? slideToPosition : default;
            set { if (SlideToPosition != value) SetData("slideToPosition", value); }
        }
    }
}
