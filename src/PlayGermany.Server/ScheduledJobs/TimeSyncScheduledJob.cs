using AltV.Net;
using PlayGermany.Server.ScheduledJobs.Base;
using System;

namespace PlayGermany.Server.ScheduledJobs
{
    public class TimeSyncScheduledJob
        : BaseScheduledJob
    {
        public DateTime CurrentDate { get; set; }

        public TimeSyncScheduledJob()
            : base(TimeSpan.FromSeconds(3d))
        {
        }

        public override void Action()
        {
            CurrentDate = DateTime.Now;

            foreach (var player in Alt.GetAllPlayers())
            {
                player.SetDateTime(CurrentDate.Day, CurrentDate.Month, CurrentDate.Year, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second);
            }
        }
    }
}
