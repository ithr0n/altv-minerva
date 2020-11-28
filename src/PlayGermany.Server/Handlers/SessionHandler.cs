using AltV.Net;
using PlayGermany.Server.Entities;

namespace PlayGermany.Server.Handlers
{
    public class SessionHandler
    {
        public SessionHandler()
        {
            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            var uiUrl = "http://resource/html/index.html";

#if DEBUG
            uiUrl = "http://localhost:8080/index.html";
#endif

            player.Emit("View:Initialize", uiUrl);
        }
    }
}
