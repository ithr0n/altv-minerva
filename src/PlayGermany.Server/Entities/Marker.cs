using AltV.Net.EntitySync;
using PlayGermany.Server.EntitySync.WritableData;
using PlayGermany.Server.Enums;
using System.Numerics;

namespace PlayGermany.Server.EntitySync
{
    /// <summary>
    /// DynamicMarker class that stores all data related to a single marker.
    /// </summary>
    public class Marker 
        : Entity, IEntity
    {
        public Marker(Vector3 position, int dimension, uint range)
            : base((ulong)EntitySyncType.Marker, position, dimension, range)
        {
        }

        #region Properties

        /// <summary>
        /// Set or get the current object's rotation (in degrees).
        /// </summary>
        public WritableVector3 Rotation
        {
            get => TryGetData("rotation", out WritableVector3 rotation) ? rotation : null;
            set { if (Rotation != value) SetData("rotation", value); }
        }

        /// <summary>
        /// Set a texture dictionary, pass null to remove.
        /// </summary>
        public string TextureDict
        {
            get => TryGetData("textureDict", out string textureDict) ? textureDict : null;
            set { if (TextureDict != value) SetData("textureDict", value); }
        }

        /// <summary>
        /// Texture name, only applicable if TextureDict is set. pass null to reset value.
        /// </summary>
        public string TextureName
        {
            get => TryGetData("textureName", out string textureName) ? textureName : null;
            set { if (TextureName != value) SetData("textureName", value); }
        }

        /// <summary>
        /// Whether the marker should rotate on the Y axis(heading).
        /// </summary>
        public bool Rotate
        {
            get => TryGetData("rotate", out bool rotate) ? rotate : false;
            set { if (Rotate != value) SetData("rotate", value); }
        }

        /// <summary>
        /// Whether the marker should be drawn onto the entity when they enter it.
        /// </summary>
        public bool DrawOnEnter
        {
            get => TryGetData("drawOnEnter", out bool drawOnEnter) ? drawOnEnter : false;
            set { if (DrawOnEnter != value) SetData("drawOnEnter", value); }
        }

        /// <summary>
        /// Whether the marker should rotate on the Y axis towards the player's camera.
        /// </summary>
        public bool FaceCamera
        {
            get => TryGetData("faceCam", out bool faceCam) ? faceCam : false;
            set { if (FaceCamera != value) SetData("faceCam", value); }
        }

        /// <summary>
        /// Whether the marker should bob up and down.
        /// </summary>
        public bool BobUpDown
        {
            get => TryGetData("bobUpDown", out bool bobUpDown) ? bobUpDown : false;
            set { if (BobUpDown != value) SetData("bobUpDown", value); }
        }

        /// <summary>
        /// Set scale of the marker.
        /// </summary>
        public WritableVector3 Scale
        {
            get => TryGetData("scale", out WritableVector3 scale) ? scale : default;
            set { if (Scale != value) SetData("scale", value); }
        }

        /// <summary>
        /// Represents a heading on each axis in which the marker should face, alternatively you can rotate each axis independently with Rotation and set Direction axis to 0.
        /// </summary>
        public WritableVector3 Direction
        {
            get => TryGetData("direction", out WritableVector3 direction) ? direction : default;
            set { if (Direction != value) SetData("direction", value); }
        }

        /// <summary>
        /// Set or get the current marker's type(see MarkerTypes enum).
        /// </summary>
        public MarkerType MarkerType
        {
            get => TryGetData("markerType", out MarkerType markerType) ? markerType : default;
            set { if (MarkerType != value) SetData("markerType", value); }
        }

        /// <summary>
        /// Set marker color.
        /// </summary>
        public WritableRgba Color
        {
            get => TryGetData("color", out WritableRgba color) ? color : default;
            set { if (Color != value) SetData("color", value); }
        }

        #endregion
    }
}
