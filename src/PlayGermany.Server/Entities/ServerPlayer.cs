using AltV.Net.Elements.Entities;
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
            set => SetStreamSyncedMetaData("hunger", value);
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
            set => SetStreamSyncedMetaData("thirst", value);
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
    }
}