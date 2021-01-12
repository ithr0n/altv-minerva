using Microsoft.Extensions.Logging;
using Minerva.Server.EntitySync.Entities;

namespace Minerva.Server.EntitySync.Streamers
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
