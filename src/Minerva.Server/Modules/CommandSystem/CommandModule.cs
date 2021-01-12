using AltV.Net;
using AltV.Net.Elements.Args;
using AltV.Net.Elements.Entities;
using AltV.Net.FunctionParser;
using Minerva.Server.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Minerva.Server.Modules.CommandSystem
{
    public class CommandModule 
        : IModule
    {
        #region CommandDoesNotExists

        private static readonly HashSet<CommandDoesNotExistDelegate> CommandDoesNotExistDelegates =
            new HashSet<CommandDoesNotExistDelegate>();

        public delegate void CommandDoesNotExistDelegate(ServerPlayer player, string command);

        public static event CommandDoesNotExistDelegate OnCommandDoesNotExist
        {
            add => CommandDoesNotExistDelegates.Add(value);
            remove => CommandDoesNotExistDelegates.Remove(value);
        }

        #endregion

        #region CommandAccessLevelViolation

        private static readonly HashSet<CommandAccessLevelViolationDelegate> CommandAccessLevelViolationDelegates =
            new HashSet<CommandAccessLevelViolationDelegate>();

        public delegate void CommandAccessLevelViolationDelegate(ServerPlayer player, string command);

        public static event CommandAccessLevelViolationDelegate OnCommandAccessLevelViolation
        {
            add => CommandAccessLevelViolationDelegates.Add(value);
            remove => CommandAccessLevelViolationDelegates.Remove(value);
        }

        #endregion

        private delegate void CommandDelegate(ServerPlayer player, string[] arguments);

        private static readonly LinkedList<Function> Functions = new LinkedList<Function>();

        private static readonly LinkedList<GCHandle> Handles = new LinkedList<GCHandle>();

        private readonly IDictionary<string, LinkedList<CommandDelegate>> _commandDelegates =
            new Dictionary<string, LinkedList<CommandDelegate>>();

        private static readonly string[] EmptyArgs = new string[0];

        public void OnScriptsStarted(IScript[] scripts)
        {
            foreach (var script in scripts)
                RegisterEvents(script);

            Alt.OnClient<ServerPlayer, string>("Commands:Execute", OnCommandRequest, OnCommandRequestParser);
        }

        public void OnStop()
        {
            Functions.Clear();

            foreach (var handle in Handles)
            {
                handle.Free();
            }

            Handles.Clear();
        }

        private static void OnCommandRequestParser(IPlayer player, MValueConst[] valueArray,
            Action<ServerPlayer, string> action)
        {
            if (valueArray.Length != 1) return;
            var arg = valueArray[0];
            if (arg.type != MValueConst.Type.String) return;
            action((ServerPlayer)player, arg.GetString());
        }

        private void OnCommandRequest(ServerPlayer player, string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            var args = command.Split(' ');
            var argsLength = args.Length;

            if (argsLength < 1) return; // should never happen

            var cmd = args[0];

            LinkedList<CommandDelegate> delegates;
            if (argsLength < 2)
            {
                if (_commandDelegates.TryGetValue(cmd, out delegates) && delegates.Count > 0)
                {
                    foreach (var commandDelegate in delegates)
                    {
                        commandDelegate(player, EmptyArgs);
                    }
                }
                else
                {
                    foreach (var doesNotExistDelegate in CommandDoesNotExistDelegates)
                    {
                        doesNotExistDelegate(player, cmd);
                    }
                }

                return;
            }

            var argsArray = new string[argsLength - 1];
            Array.Copy(args, 1, argsArray, 0, argsLength - 1);
            if (_commandDelegates.TryGetValue(cmd, out delegates) && delegates.Count > 0)
            {
                foreach (var commandDelegate in delegates)
                {
                    commandDelegate(player, argsArray);
                }
            }
            else
            {
                foreach (var doesNotExistDelegate in CommandDoesNotExistDelegates)
                {
                    doesNotExistDelegate(player, cmd);
                }
            }
        }

        private void RegisterEvents(object target)
        {
            ModuleScriptMethodIndexer.Index(
                target,
                new[] { typeof(Command), typeof(CommandEvent) },
                (baseEvent, eventMethod, eventMethodDelegate) =>
                {
                    switch (baseEvent)
                    {
                        case Command command:
                            {
                                var commandName = command.Name ?? eventMethod.Name;

                                Handles.AddLast(GCHandle.Alloc(eventMethodDelegate));

                                var function = Function.Create(eventMethodDelegate);

                                if (function == null)
                                {
                                    Console.WriteLine($"Unsupported Command method: {eventMethod}");
                                    return;
                                }

                                Functions.AddLast(function);

                                if (!_commandDelegates.TryGetValue(commandName, out var delegates))
                                {
                                    delegates = new LinkedList<CommandDelegate>();
                                    _commandDelegates[commandName] = delegates;
                                }

                                if (command.GreedyArg)
                                {
                                    delegates.AddLast((player, arguments) =>
                                    {
                                        function.Call(player, new[] { string.Join(" ", arguments) });
                                    });
                                }
                                else
                                {
                                    delegates.AddLast((player, arguments) => { function.Call(player, arguments); });
                                }

                                var aliases = command.Aliases;
                                if (aliases != null)
                                {
                                    foreach (var alias in aliases)
                                    {
                                        if (!_commandDelegates.TryGetValue(alias, out delegates))
                                        {
                                            delegates = new LinkedList<CommandDelegate>();
                                            _commandDelegates[alias] = delegates;
                                        }

                                        if (command.GreedyArg)
                                        {
                                            delegates.AddLast((player, arguments) =>
                                            {
                                                function.Call(player, new[] { string.Join(" ", arguments) });
                                            });
                                        }
                                        else
                                        {
                                            delegates.AddLast((player, arguments) => { function.Call(player, arguments); });
                                        }
                                    }
                                }

                                break;
                            }

                        case CommandEvent commandEvent:
                            {
                                var commandEventType = commandEvent.EventType;
                                ScriptFunction scriptFunction;

                                switch (commandEventType)
                                {
                                    case CommandEventType.NotFound:
                                        scriptFunction = ScriptFunction.Create(eventMethodDelegate,
                                            new[] { typeof(ServerPlayer), typeof(string) });

                                        if (scriptFunction == null) return;

                                        OnCommandDoesNotExist += (player, commandName) =>
                                        {
                                            scriptFunction.Set(player);
                                            scriptFunction.Set(commandName);
                                            scriptFunction.Call();
                                        };

                                        break;

                                    case CommandEventType.AccessLevelViolation:
                                        scriptFunction = ScriptFunction.Create(eventMethodDelegate,
                                            new[] { typeof(ServerPlayer), typeof(string) });

                                        if (scriptFunction == null) return;

                                        OnCommandAccessLevelViolation += (player, commandName) =>
                                        {
                                            scriptFunction.Set(player);
                                            scriptFunction.Set(commandName);
                                            scriptFunction.Call();
                                        };

                                        break;
                                }

                                break;
                            }
                    }
                });
        }
    }
}
