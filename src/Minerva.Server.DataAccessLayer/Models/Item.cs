using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.DataAccessLayer.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemDefinitionId { get; set; }
        public ItemDefinition ItemDefinition { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}