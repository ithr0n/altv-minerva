using System;
using AltV.Net.Elements.Entities;
using Minerva.Server.Core.Contracts.Enums;
using Minerva.Server.Core.Contracts.Models;
using Minerva.Server.Core.PlayerBuffs;
using Minerva.Server.Core.PlayerJobs;

namespace Minerva.Server.Core.Entities
{
    public class ServerPlayer
        : Player
    {
        public ServerPlayer(IntPtr nativePointer, ushort id)
            : base(nativePointer, id)
        {
            Hunger = 100;
            Thirst = 100;

            Buffs = new BuffCollection(this);
        }

        public string RoleplayName
        {
            get
            {
                if (!GetStreamSyncedMetaData("roleplayName", out string result))
                {
                    if (Character != null)
                    {
                        SetStreamSyncedMetaData("roleplayName", Character.Name);
                        return Character.Name;
                    }

                    return string.Empty;
                }

                return result;
            }
        }

        public uint Cash
        {
            get
            {
                if (!GetStreamSyncedMetaData("cash", out uint result))
                {
                    return 0;
                }

                return result;
            }
            set => SetStreamSyncedMetaData("cash", value);
        }

        public ushort Hunger
        {
            get
            {
                if (!GetStreamSyncedMetaData("hunger", out ushort result))
                {
                    return 0;
                }

                return result;
            }
            set
            {
                var newValue = Math.Min(value, (ushort)100);

                SetStreamSyncedMetaData("hunger", newValue);
            }
        }

        public ushort Thirst
        {
            get
            {
                if (!GetStreamSyncedMetaData("thirst", out ushort result))
                {
                    return 0;
                }

                return result;
            }
            set
            {
                var newValue = Math.Min(value, (ushort)100);

                SetStreamSyncedMetaData("thirst", newValue);
            }
        }

        public PlayerVoiceLevel VoiceLevel
        {
            get
            {
                if (!GetStreamSyncedMetaData("voiceIndex", out int result))
                {
                    return PlayerVoiceLevel.Mute;
                }

                return (PlayerVoiceLevel)result;
            }
            set
            {
                SetStreamSyncedMetaData("voiceIndex", (int)value);
            }
        }

        public Account Account { get; set; }

        public Character Character { get; set; }

        public BuffCollection Buffs { get; }

        public PlayerJob CurrentJob { get; set; }

        public bool IsLoggedIn => IsConnected && Account != null;

        public bool IsSpawned => IsLoggedIn && Character != null;
    }
}
