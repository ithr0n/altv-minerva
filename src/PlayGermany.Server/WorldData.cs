using AltV.Net;
using AltV.Net.Enums;
using System;

namespace PlayGermany.Server
{
    public class WorldData
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
            get
            {
                if (!Alt.GetSyncedMetaData("clockPaused", out bool clockPaused))
                {
                    return false;
                }

                return clockPaused;
            }
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
            get
            {
                if (!Alt.GetSyncedMetaData("blackout", out bool blackout))
                {
                    return false;
                }

                return blackout;
            }
            set => Alt.SetSyncedMetaData("blackout", value);
        }
    }
}
