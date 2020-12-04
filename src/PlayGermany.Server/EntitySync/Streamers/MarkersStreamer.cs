using Microsoft.Extensions.Logging;
using PlayGermany.Server.EntitySync.WritableData;
using PlayGermany.Server.Enums;
using System.Numerics;

namespace PlayGermany.Server.EntitySync.Streamers
{
    public class MarkersStreamer
        : Streamer<Marker>
    {
        public MarkersStreamer(ILogger<MarkersStreamer> logger)
            : base(logger)
        {
        }

        public ulong Create(
            MarkerType markerType,
            Vector3 position,
            Vector3 scale,
            Vector3? rotation = null,
            Vector3? direction = null,
            WritableRgba color = null,
            int dimension = 0,
            bool? bobUpDown = false,
            bool? faceCamera = false,
            bool? rotate = false,
            string textureDict = null,
            string textureName = null,
            bool? drawOnEnter = false,
            uint streamRange = 100
        )
        {
            var newEntity = new Marker(position, dimension, streamRange)
            {
                MarkerType = markerType,
                Scale = new WritableVector3(scale),
                Rotation = rotation.HasValue ? new WritableVector3(rotation.Value) : new WritableVector3(),
                Direction = direction.HasValue ? new WritableVector3(direction.Value) : new WritableVector3(),
                Color = color ?? null,
                BobUpDown = bobUpDown ?? false,
                FaceCamera = faceCamera ?? false,
                Rotate = rotate ?? false,
                TextureDict = textureDict ?? null,
                TextureName = textureName ?? null,
                DrawOnEnter = drawOnEnter ?? false
            };

            return Create(newEntity);
        }
    }
}
