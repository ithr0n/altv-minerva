using PlayGermany.Server.DataAccessLayer.Models.Base;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class Character
        : PositionRotationEntityBase
    {
        public Account Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ushort Health { get; set; }
        public ushort Armor { get; set; }
        public decimal Cash { get; set; }
    }
}
