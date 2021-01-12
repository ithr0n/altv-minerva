using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Minerva.Server.DataAccessLayer.Enums;

namespace Minerva.Server.DataAccessLayer.Models
{
    public class NpcShopEntry
    {
        public int NpcShopId { get; set; }
        public NpcShop NpcShop { get; set; }

        public int ItemDefinitionId { get; set; }
        public ItemDefinition ItemDefinition { get; set; }

        public NpcShopEntryType Type { get; set; }

        public decimal Price { get; set; }
    }
}