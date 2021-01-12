using Minerva.Server.ItemImplementations.Base;

namespace Minerva.Server.ItemImplementations
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