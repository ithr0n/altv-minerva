using System;

namespace Minerva.Server.Modules.CommandSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandEvent : Attribute
    {
        public CommandEventType EventType { get; }

        public CommandEvent(CommandEventType eventType)
        {
            EventType = eventType;
        }
    }
}
