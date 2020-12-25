using System;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Callbacks;
using PlayGermany.Server.Entities;
using PlayGermany.Server.ScheduledJobs.Base;

namespace PlayGermany.Server.ScheduledJobs
{
    public class CharacterStatsUpdate
        : BaseScheduledJob
        {
            private readonly Random _random;

            private ILogger<CharacterStatsUpdate> Logger { get; }

            public CharacterStatsUpdate(ILogger<CharacterStatsUpdate> logger) : base(TimeSpan.FromMinutes(1))
            {
                _random = new Random();
                Logger = logger;
            }

            public override void Action()
            {
                var callback = new FunctionCallback<IPlayer>((player) =>
                {
                    var serverPlayer = player as ServerPlayer;

                    if (serverPlayer != null && serverPlayer.IsSpawned && !serverPlayer.IsDead)
                    {
                        serverPlayer.Hunger -= Math.Max((ushort) _random.Next(2, 7), (ushort) 0);
                        serverPlayer.Thirst -= Math.Max((ushort) _random.Next(4, 11), (ushort) 0);

                        if (serverPlayer.Hunger <= 0)
                        {
                            serverPlayer.Health -= Math.Min((ushort) 10, serverPlayer.Health);
                        }

                        if (serverPlayer.Thirst <= 0)
                        {
                            serverPlayer.Health -= Math.Min((ushort) 30, serverPlayer.Health);
                        }
                    }
                });

                Alt.ForEachPlayers(callback);
            }
        }
}
