using PlayGermany.Server.ItemImplementations.Base;

namespace PlayGermany.Server.ItemImplementations
{
    public class WaterBottle
        : ConsumableItemImplementation
    {
        public WaterBottle()
            : base(thirstModifier: 10)
        {
            
        }
    }
}