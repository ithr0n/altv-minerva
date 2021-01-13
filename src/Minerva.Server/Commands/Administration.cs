using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Minerva.Server.Callbacks;
using Minerva.Server.Contracts.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Enums;
using Minerva.Server.Entities;
using Minerva.Server.Enums;
using Minerva.Server.Extensions;
using Minerva.Server.Modules.CommandSystem;
using System.Linq;

namespace Minerva.Server.Commands
{
    public class Administration
        : ISingletonScript
    {
        [Command("broadcast", AccessLevel.Admin, true)]
        public void OnTeleportToWaypointCommand(ServerPlayer player, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                player.Notify("Du musst eine Nachricht angeben!", NotificationType.Error);
                return;
            }

            Alt.EmitAllClients("UiManager:Notification", message);
        }

        [Command("tp", AccessLevel.Admin)]
        public void OnTeleportCommand(ServerPlayer player, string x, string y, string z)
        {
            if (float.TryParse(x.Trim(','), out var xPos) && float.TryParse(y.Trim(','), out var yPos) & float.TryParse(z.Trim(','), out var zPos))
            {
                player.Position = new Position(xPos, yPos, zPos);
            }
        }

        [Command("tpto", AccessLevel.Admin)]
        public void OnTeleportToPlayerCommand(ServerPlayer player, string target = null)
        {
            if (target == null)
            {
                player.Notify("Du musst ein Ziel angeben!", NotificationType.Error);
                return;
            }

            var targetPlayer = Alt.GetAllPlayers().SingleOrDefault(player => ((ServerPlayer)player).RoleplayName.Contains(target));

            if (targetPlayer != null && targetPlayer != this && targetPlayer.Exists)
            {
                player.Position = targetPlayer.Position + new Position(1, 1, 0.5f);

                if (targetPlayer.IsInVehicle)
                {
                    player.Emit("VehicleHandler:TeleportInto", targetPlayer.Vehicle);
                }
            }
            else
            {
                player.Notify("Spieler nicht gefunden oder nicht eindeutig", NotificationType.Error);
            }
        }

        [Command("tpwp", AccessLevel.Admin)]
        public void OnTeleportToWaypointCommand(ServerPlayer player)
        {
            player.Emit("ConsoleHandler:TeleportToWaypoint");
        }

        [Command("players", AccessLevel.Admin)]
        public void OnPlayersCommand(ServerPlayer player)
        {
            //var lambda = new AsyncFunctionCallback<IPlayer>(async (IPlayer otherPlayer) =>
            var lambda = new FunctionCallback<IPlayer>((otherPlayer) =>
            {
                if (otherPlayer is ServerPlayer serverPlayer)
                {
                    player.Notify(serverPlayer.RoleplayName);
                }

                //    await Task.CompletedTask;
            });

            //await Alt.ForEachPlayers(lambda);
            Alt.ForEachPlayers(lambda);
        }
    }
}
