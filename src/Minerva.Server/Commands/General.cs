using Minerva.Server.Contracts.ScriptStrategy;
using Minerva.Server.Entities;
using Minerva.Server.Enums;
using Minerva.Server.Extensions;
using Minerva.Server.Modules.CommandSystem;

namespace Minerva.Server.Commands
{
    public class General
        : IStartupSingletonScript
    {
        [CommandEvent(CommandEventType.NotFound)]
        public void OnCommandNotFound(ServerPlayer player, string commandName)
        {
            player.Notify("Ungültiger Befehl", NotificationType.Error);
        }

        [CommandEvent(CommandEventType.AccessLevelViolation)]
        public void OnCommandAccessViolation(ServerPlayer player, string commandName)
        {
            player.Notify("Fehlende Berechtigung", NotificationType.Warning);
        }
    }
}
