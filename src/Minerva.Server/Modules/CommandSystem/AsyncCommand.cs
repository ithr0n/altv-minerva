using System;

namespace Minerva.Server.Modules.CommandSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AsyncCommand
        : Attribute
    {
        public string Name { get; }

        public bool GreedyArg { get; }

        public string[] Aliases { get; }

        public AsyncCommand(string name = null, bool greedyArg = false, string[] aliases = null)
        {
            Name = name;
            GreedyArg = greedyArg;
            Aliases = aliases;
        }
    }
}
