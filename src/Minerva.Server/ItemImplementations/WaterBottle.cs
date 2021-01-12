using Minerva.Server.ItemImplementations.Base;

namespace Minerva.Server.ItemImplementations
{
    public class WaterBottle
        : FoodItemImplementation
    {
        public WaterBottle()
            : base(thirstModifier: 10)
        {

        }
    }
}