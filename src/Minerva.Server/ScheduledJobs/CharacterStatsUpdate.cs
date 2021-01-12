using System;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Minerva.Server.Callbacks;
using Minerva.Server.Entities;
using Minerva.Server.ScheduledJobs.Base;

namespace Minerva.Server.ScheduledJobs
{
    public class CharacterStatsUpdate
        : ScheduledJob
    {
        private readonly Random _random;

        public CharacterStatsUpdate()
            : base(TimeSpan.FromMinutes(1))
        {
            _random = new Random();
        }

        public override async Task Action()
        {
            var callback = new AsyncFunctionCallback<IPlayer>(async (player) =>
            {
                var serverPlayer = player as ServerPlayer;

                if (serverPlayer != null && serverPlayer.IsSpawned && !serverPlayer.IsDead)
                {
                    serverPlayer.Hunger -= Math.Max((ushort)_random.Next(2, 7), (ushort)0);
                    serverPlayer.Thirst -= Math.Max((ushort)_random.Next(4, 11), (ushort)0);

                    if (serverPlayer.Hunger <= 0)
                    {
                        serverPlayer.Health -= Math.Min((ushort)10, serverPlayer.Health);
                    }

                    if (serverPlayer.Thirst <= 0)
                    {
                        serverPlayer.Health -= Math.Min((ushort)30, serverPlayer.Health);
                    }
                }

                await Task.CompletedTask;
            });

            await Alt.ForEachPlayers(callback);
        }
    }
}
