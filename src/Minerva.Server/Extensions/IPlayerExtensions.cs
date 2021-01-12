using AltV.Net.Elements.Entities;
using Minerva.Server.Enums;

namespace Minerva.Server.Extensions
{
    public static class IPlayerExtensions
    {
        public static void Notify(this IPlayer player, string message, NotificationType notificationType = NotificationType.Notification)
        {
            switch (notificationType)
            {
                case NotificationType.Notification:
                default:
                    player.Emit("UiManager:Notification", message);
                    break;

                case NotificationType.Info:
                    player.Emit("UiManager:Info", message);
                    break;

                case NotificationType.Success:
                    player.Emit("UiManager:Success", message);
                    break;

                case NotificationType.Warning:
                    player.Emit("UiManager:Warning", message);

                    break;
                case NotificationType.Error:
                    player.Emit("UiManager:Error", message);
                    break;
            }
        }
    }
}
