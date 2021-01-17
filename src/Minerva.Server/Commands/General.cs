using Minerva.Server.Core.CommandSystem;
using Minerva.Server.Extensions;
using Minerva.Server.Core.ScriptStrategy;
using Minerva.Server.Core.Entities;
using Minerva.Server.Core.Contracts.Enums;

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
