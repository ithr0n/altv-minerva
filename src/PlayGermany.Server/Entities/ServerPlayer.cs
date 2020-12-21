using AltV.Net.Elements.Entities;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.Enums;
using System;

namespace PlayGermany.Server.Entities
{
    public class ServerPlayer
        : Player
    {
        public ServerPlayer(IntPtr nativePointer, ushort id)
            : base(nativePointer, id)
        {
            Hunger = 100;
            Thirst = 100;
        }

        public string RoleplayName
        {
            get
            {
                if (!GetStreamSyncedMetaData("roleplayName", out string result))
                {
                    return string.Empty;
                }

                return result;
            }
            set => SetStreamSyncedMetaData("roleplayName", value);
        }

        public decimal Cash
        {
            get
            {
                if (!GetStreamSyncedMetaData("cash", out decimal result))
                {
                    return 0;
                }

                return result;
            }
            set => SetStreamSyncedMetaData("cash", value);
        }

        public int Hunger
        {
            get
            {
                if (!GetStreamSyncedMetaData("hunger", out int result))
                {
                    return 0;
                }

                return result;
            }
            set
            {
                var newValue = Math.Min(value, 0);
                newValue = Math.Max(newValue, 100);

                SetStreamSyncedMetaData("hunger", newValue);
            }
        }

        public int Thirst
        {
            get
            {
                if (!GetStreamSyncedMetaData("thirst", out int result))
                {
                    return 0;
                }

                return result;
            }
            set
            {
                var newValue = Math.Min(value, 0);
                newValue = Math.Max(newValue, 100);

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

        public bool LoggedIn => Account != null;
    }
}