using AltV.Net.Elements.Entities;
using AltV.Net.Interactions;
using Minerva.Server.Core.Contracts.Enums;
using Minerva.Server.Core.Entities;
using Minerva.Server.Extensions;
using System.Numerics;

namespace Minerva.Server.Interactions
{
    public class AtmInteraction
        : Interaction
    {
        public AtmInteraction(ulong id, Vector3 position, int dimension, uint range) 
            : base((ulong)InteractionType.Atm, id, position, dimension, range)
        {
        }

        public override bool OnInteraction(IPlayer player, Vector3 interactionPosition, int interactionDimension)
        {
            if (player != null && player.Exists && player is ServerPlayer serverPlayer && serverPlayer.IsLoggedIn)
            {
                // open ui
                serverPlayer.Notify("todo: open atm ui");

                return true;
            }

            return false;
        }
    }
}
