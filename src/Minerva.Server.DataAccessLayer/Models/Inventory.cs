using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.DataAccessLayer.Models
{
    public class Inventory
    {
        public Inventory()
        {
            Items = new List<Item>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float MaxWeight { get; set; }

        public List<Item> Items { get; set; }
    }
}