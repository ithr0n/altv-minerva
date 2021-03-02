using System.Numerics;
using AltV.Net;
using Microsoft.Extensions.Logging;
using Minerva.Server.EntitySync.Entities;
using Minerva.Server.EntitySync.WritableData;
using Minerva.Server.Enums;
using Minerva.Server.Modules.EntitySync.Entities;

namespace Minerva.Server.Modules.EntitySync.Streamers
{
    public class PropsStreamer
        : Streamer<Prop>
    {
        public PropsStreamer(ILogger<PropsStreamer> logger)
            : base(logger)
        {
        }

        /// <summary>
        /// Create a new dynamic object.
        /// </summary>
        /// <param name="model">The object model name.</param>
        /// <param name="position">The position to spawn the object at.</param>
        /// <param name="rotation">The rotation to spawn the object at(degrees).</param>
        /// <param name="dimension">The dimension to spawn the object in.</param>
        /// <param name="isDynamic">(Optional): Set object dynamic or not.</param>
        /// <param name="placeObjectOnGroundProperly"></param>
        /// <param name="frozen">(Optional): Set object frozen.</param>
        /// <param name="lodDistance">(Optional): Set LOD distance.</param>
        /// <param name="lightColor">(Optional): set light color.</param>
        /// <param name="onFire">(Optional): set object on fire(DOESN'T WORK PROPERLY YET!)</param>
        /// <param name="textureVariation">(Optional): Set object texture variation.</param>
        /// <param name="visible">(Optional): Set object visibility.</param>
        /// <param name="streamRange">(Optional): The range that a player has to be in before the object spawns, default value is 400.</param>
        /// <returns>The newly created dynamic object.</returns>
        public Prop Create(
            string model,
            Vector3 position,
            Vector3 rotation,
            int dimension = 0,
            bool? isDynamic = null,
            bool? placeObjectOnGroundProperly = false,
            bool? frozen = null,
            uint? lodDistance = null,
            WritableRgb lightColor = null,
            bool? onFire = null,
            TextureVariation? textureVariation = null,
            bool? visible = null,
            bool? isCollisionless = null,
            uint streamRange = 520
        )
        {
            return Create(Alt.Hash(model), position, rotation, dimension, isDynamic, placeObjectOnGroundProperly, frozen, lodDistance, lightColor, onFire, textureVariation, visible, isCollisionless, streamRange);
        }

        /// <summary>
        /// Create a new dynamic object.
        /// </summary>
        /// <param name="model">The object model name.</param>
        /// <param name="position">The position to spawn the object at.</param>
        /// <param name="rotation">The rotation to spawn the object at(degrees).</param>
        /// <param name="dimension">The dimension to spawn the object in.</param>
        /// <param name="isDynamic">(Optional): Set object dynamic or not.</param>
        /// <param name="placeObjectOnGroundProperly"></param>
        /// <param name="frozen">(Optional): Set object frozen.</param>
        /// <param name="lodDistance">(Optional): Set LOD distance.</param>
        /// <param name="lightColor">(Optional): set light color.</param>
        /// <param name="onFire">(Optional): set object on fire(DOESN'T WORK PROPERLY YET!)</param>
        /// <param name="textureVariation">(Optional): Set object texture variation.</param>
        /// <param name="visible">(Optional): Set object visibility.</param>
        /// <param name="streamRange">(Optional): The range that a player has to be in before the object spawns, default value is 400.</param>
        /// <returns>The newly created dynamic object.</returns>
        public Prop Create(
            uint model,
            Vector3 position,
            Vector3 rotation,
            int dimension = 0,
            bool? isDynamic = null,
            bool? placeObjectOnGroundProperly = false,
            bool? frozen = null,
            uint? lodDistance = null,
            WritableRgb lightColor = null,
            bool? onFire = null,
            TextureVariation? textureVariation = null,
            bool? visible = null,
            bool? isCollisionless = null,
            uint streamRange = 520
        )
        {
            var newEntity = new Prop(position, dimension, streamRange)
            {
                Rotation = new WritableVector3(rotation),
                Model = model,
                Dynamic = isDynamic,
                Frozen = frozen,
                LodDistance = lodDistance,
                LightColor = lightColor,
                OnFire = onFire,
                TextureVariation = textureVariation,
                Visible = visible,
                IsCollisionless = isCollisionless,
                PlacedOnGround = placeObjectOnGroundProperly
            };

            return Create(newEntity);
        }
    }
}
