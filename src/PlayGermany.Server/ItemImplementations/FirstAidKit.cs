using PlayGermany.Server.ItemImplementations.Base;

namespace PlayGermany.Server.ItemImplementations
{
    public class FirstAidKit
        : ConsumableItemImplementation
    {
        public FirstAidKit()
            : base(healthModifier: 30)
        {
        }
    }
}