using AltV.Net;
using Microsoft.Extensions.Logging;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.Handlers
{
    public class SessionHandler
    {
        public ILogger<SessionHandler> Logger { get; }

        public SessionHandler(ILogger<SessionHandler> logger)
        {
            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
            Logger = logger;
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            var uiUrl = "http://resource/html/index.html";

#if DEBUG
            uiUrl = "http://localhost:8080/index.html";
#endif

            Logger.LogDebug("Requesting UI from {url}", uiUrl);

            player.Emit("View:Initialize", uiUrl);
        }
    }
}
