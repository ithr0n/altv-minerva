using AltV.Net.EntitySync;
using PlayGermany.Server.EntitySync.DataTypes;
using PlayGermany.Server.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PlayGermany.Server.EntitySync
{
    public class PropEntity
        : Entity
    {
        public PropEntity(ulong type, Vector3 position, int dimension, uint range)
            : base((ulong)EntityTypes.Prop, position, dimension, range)
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
        public Vector3 Rotation
        {
            get
            {
                if (TryGetData("rotation", out Dictionary<string, object> data))
                {
                    return new Vector3()
                    {
                        X = Convert.ToSingle(data["X"]),
                        Y = Convert.ToSingle(data["Y"]),
                        Z = Convert.ToSingle(data["Z"])
                    };
                }

                return default;
            }
            set
            {
                //Abort sending not needed update if new data is same as old
                if (Rotation.X == value.X && Rotation.Y == value.Y && Rotation.Z == value.Z)
                    return;

                var rotationDataDict = new Dictionary<string, object>()
                {
                    ["x"] = value.X,
                    ["y"] = value.Y,
                    ["z"] = value.Z,
                };

                SetData("rotation", rotationDataDict);
            }
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
        public Rgb LightColor
        {
            get => TryGetData("lightColor", out Rgb lightColor) ? lightColor : default;
            set { if (LightColor != value) SetData("lightColor", value); }
        }

        public Vector3 Velocity
        {
            get
            {
                if (TryGetData("velocity", out Dictionary<string, object> data))
                {
                    return new Vector3()
                    {
                        X = Convert.ToSingle(data["x"]),
                        Y = Convert.ToSingle(data["y"]),
                        Z = Convert.ToSingle(data["z"]),
                    };
                }

                return default;
            }
            set
            {
                // No data changed
                if (Velocity.X == value.X && Velocity.Y == value.Y && Velocity.Z == value.Z)
                    return;

                var dict = new Dictionary<string, object>()
                {
                    ["x"] = value.X,
                    ["y"] = value.Y,
                    ["z"] = value.Z,
                };

                SetData("velocity", dict);
            }
        }

        public Vector3 SlideToPosition
        {
            get
            {
                if (TryGetData("SlideToPosition", out Dictionary<string, object> data))
                {
                    return new Vector3()
                    {
                        X = Convert.ToSingle(data["x"]),
                        Y = Convert.ToSingle(data["y"]),
                        Z = Convert.ToSingle(data["z"]),
                    };
                }

                return default;
            }
            set
            {
                // No data changed
                if (SlideToPosition.X == value.X && SlideToPosition.Y == value.Y && SlideToPosition.Z == value.Z)
                    return;

                var dict = new Dictionary<string, object>()
                {
                    ["x"] = value.X,
                    ["y"] = value.Y,
                    ["z"] = value.Z,
                };

                SetData("SlideToPosition", dict);
            }
        }
    }
}
