using Minerva.Server.Core.Contracts.Enums;
using System;

namespace Minerva.Server.Core.CommandSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Command
        : Attribute
    {
        public string Name { get; }

        public AccessLevel RequiredAccessLevel { get; }

        public bool GreedyArg { get; }

        public string[] Aliases { get; }

        public Command(string name = null, AccessLevel requiredAccessLevel = AccessLevel.Player, bool greedyArg = false, string[] aliases = null)
        {
            Name = name;
            RequiredAccessLevel = requiredAccessLevel;
            GreedyArg = greedyArg;
            Aliases = aliases;
        }
    }
}
