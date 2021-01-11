using Microsoft.Extensions.Logging;
using PlayGermany.Server.EntitySync.Entities;

namespace PlayGermany.Server.EntitySync.Streamers
{
    public class PedsStreamer
        : Streamer<Ped>
    {
        public PedsStreamer(ILogger<PedsStreamer> logger)
            : base(logger)
        {
        }
    }
}
