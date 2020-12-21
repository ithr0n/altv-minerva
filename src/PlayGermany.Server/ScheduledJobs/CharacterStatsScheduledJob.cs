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
    public class CharacterStatsScheduledJob
        : BaseScheduledJob
        {
            private readonly Random _random;

            private ILogger<CharacterStatsScheduledJob> Logger { get; }

            public CharacterStatsScheduledJob(ILogger<CharacterStatsScheduledJob> logger) : base(TimeSpan.FromMinutes(1))
            {
                _random = new Random();
                Logger = logger;
            }

            public override void Action()
            {
                var callback = new FunctionCallback<IPlayer>((player) =>
                {
                    var serverPlayer = player as ServerPlayer;

                    if (serverPlayer != null)
                    {
                        var newHunger = serverPlayer.Hunger - _random.Next(2, 7);
                        var newThirst = serverPlayer.Thirst - _random.Next(4, 11);

                        serverPlayer.Hunger = Math.Max(newHunger, 0);
                        serverPlayer.Thirst = Math.Max(newThirst, 0);

                        if (serverPlayer.Hunger == 0)
                        {
                            serverPlayer.Health -= 10;
                        }

                        if (serverPlayer.Thirst == 0)
                        {
                            serverPlayer.Health -= 30;
                        }
                    }
                });

                Alt.ForEachPlayers(callback);
            }
        }
}
