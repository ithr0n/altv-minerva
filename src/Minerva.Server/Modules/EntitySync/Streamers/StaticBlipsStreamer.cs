﻿using System.Numerics;
using Microsoft.Extensions.Logging;
using Minerva.Server.Core.Contracts.Abstractions.ScriptStrategy;
using Minerva.Server.EntitySync.Entities;

namespace Minerva.Server.Modules.EntitySync.Streamers
{
    public class StaticBlipsStreamer
        : Streamer<StaticBlip>, ISingletonScript
    {
        public StaticBlipsStreamer(ILogger<StaticBlipsStreamer> logger)
            : base(logger)
        {
        }

        /// <summary>
        /// Create static blip without any range limit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spriteId"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="shortRange"></param>
        /// <param name="dimension"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public StaticBlip Create(
            string name,
            int spriteId,
            Vector3 position,
            int color,
            float scale,
            bool shortRange,
            int dimension = 0,
            uint range = 100
        )
        {
            var newEntity = new StaticBlip(position, dimension, range)
            {
                Name = name,
                Color = color,
                Scale = scale,
                ShortRange = shortRange,
                Sprite = spriteId,
            };

            return Create(newEntity);
        }
    }
}
