using PlayGermany.Server.ItemTypes.Base;

namespace PlayGermany.Server.ItemTypes
{
    public class WaterBottle
        : ConsumableItemTypeBase
    {
        public WaterBottle()
            : base(thirstModifier: 10)
        {
            
        }
    }
}