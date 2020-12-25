using System;
using System.Collections.Generic;
using AltV.Net.Elements.Entities;
using PlayGermany.Server.DataAccessLayer.Models;
using PlayGermany.Server.Enums;
using PlayGermany.Server.PlayerBuffs.Base;

namespace PlayGermany.Server.Entities
{
    public class ServerPlayer
        : Player
        {
            public ServerPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
            {
                Hunger = 100;
                Thirst = 100;

                _buffs = new List<PlayerBuff>();
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
                set
                {
                    var newValue = Math.Min(value, (ushort) 100);

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
                    var newValue = Math.Min(value, (ushort) 100);

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

                    return (PlayerVoiceLevel) result;
                }
                set
                {
                    SetStreamSyncedMetaData("voiceIndex", (int) value);
                }
            }

            public Account Account { get; set; }

            public Character Character { get; set; }

            public bool IsLoggedIn => IsConnected && Account != null;
            
            public bool IsSpawned => IsLoggedIn && Character != null;

            #region Buffs

            private List<PlayerBuff> _buffs;

            public void ApplyBuff(PlayerBuff buff)
            {
                buff.OnApplied(this);
                _buffs.Add(buff);
            }

            public bool RemoveBuff(PlayerBuff buff)
            {
                if (!_buffs.Contains(buff))
                {
                    return false;
                }

                buff.OnRemoved(this);
                _buffs.Remove(buff);

                return true;
            }

            #endregion
        }
}
