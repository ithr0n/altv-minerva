using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlayGermany.Server.DataAccessLayer.Models.Base;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class Vehicle
        : PositionRotationEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Model { get; set; }

        public decimal Mileage { get; set; }

        public decimal Fuel { get; set; }

        public decimal FuelMax { get; set; }

        public string NumberPlate { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}