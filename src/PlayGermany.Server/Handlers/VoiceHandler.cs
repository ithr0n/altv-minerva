using AltV.Net;
using AltV.Net.Elements.Entities;
using PlayGermany.Server.Entities;
using PlayGermany.Server.Enums;

namespace PlayGermany.Server.Handlers
{
    public class VoiceHandler
    {
        public IVoiceChannel MainNormalVoiceChannel { get; }
        public IVoiceChannel MainWhisperVoiceChannel { get; }
        public IVoiceChannel MainShoutVoiceChannel { get; }
        public IVoiceChannel PoliceMegaphoneVoiceChannel { get; }

        public VoiceHandler()
        {
            MainNormalVoiceChannel = Alt.CreateVoiceChannel(true, 8f);
            MainWhisperVoiceChannel = Alt.CreateVoiceChannel(true, 3f);
            MainShoutVoiceChannel = Alt.CreateVoiceChannel(true, 15f);
            PoliceMegaphoneVoiceChannel = Alt.CreateVoiceChannel(true, 32f);

            Alt.OnPlayerConnect += (player, reason) => OnPlayerConnect(player as ServerPlayer, reason);
            Alt.OnPlayerDisconnect += (player, reason) => OnPlayerDisconnect(player as ServerPlayer, reason);
            Alt.OnClient<ServerPlayer>("Voice:SwitchVoiceLevel", OnSwitchVoiceLevel);
        }

        private void OnPlayerConnect(ServerPlayer player, string reason)
        {
            MainNormalVoiceChannel.AddPlayer(player);

            MainWhisperVoiceChannel.AddPlayer(player);
            MainWhisperVoiceChannel.MutePlayer(player);

            MainShoutVoiceChannel.AddPlayer(player);
            MainShoutVoiceChannel.MutePlayer(player);

            PoliceMegaphoneVoiceChannel.AddPlayer(player);
            PoliceMegaphoneVoiceChannel.MutePlayer(player);

            player.VoiceLevel = PlayerVoiceLevel.Normal;
        }

        private void OnPlayerDisconnect(IPlayer player, string reason)
        {
            MainNormalVoiceChannel.RemovePlayer(player);
            MainWhisperVoiceChannel.RemovePlayer(player);
            MainShoutVoiceChannel.RemovePlayer(player);
            PoliceMegaphoneVoiceChannel.RemovePlayer(player);
        }

        private void OnSwitchVoiceLevel(ServerPlayer player)
        {
            var currentLevel = player.VoiceLevel;

            switch (currentLevel)
            {
                case PlayerVoiceLevel.Mute:
                    ChangeVoiceLevel(player, PlayerVoiceLevel.Whisper);
                    break;

                case PlayerVoiceLevel.Whisper:
                    ChangeVoiceLevel(player, PlayerVoiceLevel.Normal);
                    break;

                case PlayerVoiceLevel.Normal:
                    ChangeVoiceLevel(player, PlayerVoiceLevel.Shout);
                    break;

                case PlayerVoiceLevel.Shout:
                    ChangeVoiceLevel(player, PlayerVoiceLevel.Mute);
                    break;
            }
        }

        public void ChangeVoiceLevel(ServerPlayer player, PlayerVoiceLevel newLevel)
        {
            switch (newLevel)
            {
                case PlayerVoiceLevel.Mute:
                    MainWhisperVoiceChannel.MutePlayer(player);
                    MainNormalVoiceChannel.MutePlayer(player);
                    MainShoutVoiceChannel.MutePlayer(player);
                    PoliceMegaphoneVoiceChannel.MutePlayer(player);
                    break;

                case PlayerVoiceLevel.Whisper:
                    MainWhisperVoiceChannel.UnmutePlayer(player);
                    MainNormalVoiceChannel.MutePlayer(player);
                    MainShoutVoiceChannel.MutePlayer(player);
                    PoliceMegaphoneVoiceChannel.MutePlayer(player);
                    break;

                case PlayerVoiceLevel.Normal:
                    MainWhisperVoiceChannel.MutePlayer(player);
                    MainNormalVoiceChannel.UnmutePlayer(player);
                    MainShoutVoiceChannel.MutePlayer(player);
                    PoliceMegaphoneVoiceChannel.MutePlayer(player);
                    break;

                case PlayerVoiceLevel.Shout:
                    MainWhisperVoiceChannel.MutePlayer(player);
                    MainNormalVoiceChannel.MutePlayer(player);
                    MainShoutVoiceChannel.UnmutePlayer(player);
                    PoliceMegaphoneVoiceChannel.MutePlayer(player);
                    break;

                case PlayerVoiceLevel.PoliceMegaphone:
                    MainWhisperVoiceChannel.MutePlayer(player);
                    MainNormalVoiceChannel.MutePlayer(player);
                    MainShoutVoiceChannel.MutePlayer(player);
                    PoliceMegaphoneVoiceChannel.UnmutePlayer(player);
                    break;
            }

            player.VoiceLevel = newLevel;
        }
    }
}
