using AltV.Net;
using AltV.Net.Enums;
using Minerva.Server.Core.ScriptStrategy;
using System;

namespace Minerva.Server
{
    public class WorldData
        : ISingletonScript
    {
        public WorldData()
        {
            Clock = DateTime.Now;
            ClockPaused = false;
            Weather = WeatherType.Clear;
        }

        public DateTime Clock { get; set; }

        public bool ClockPaused
        {
            get => Alt.GetSyncedMetaData("clockPaused", out bool clockPaused) && clockPaused;
            set => Alt.SetSyncedMetaData("clockPaused", value);
        }

        public WeatherType Weather
        {
            get
            {
                if (!Alt.GetSyncedMetaData("weather", out uint weatherId) || !Enum.IsDefined(typeof(WeatherType), weatherId))
                {
                    return WeatherType.Clear;
                }

                return (WeatherType)weatherId;
            }
            set => Alt.SetSyncedMetaData("weather", (uint)value);
        }

        public bool Blackout
        {
            get => Alt.GetSyncedMetaData("blackout", out bool blackout) && blackout;
            set => Alt.SetSyncedMetaData("blackout", value);
        }
    }
}
