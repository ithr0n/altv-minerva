using System;

namespace Minerva.Server.Core.CommandSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandEvent
        : Attribute
    {
        public CommandEventType EventType { get; }

        public CommandEvent(CommandEventType eventType)
        {
            EventType = eventType;
        }
    }
}
