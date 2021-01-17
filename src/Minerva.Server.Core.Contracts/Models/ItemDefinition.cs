using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.Core.Contracts.Models
{
    public class ItemDefinition
    {
        public ItemDefinition()
        {
            AllItems = new List<Item>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemImplementationType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Weight { get; set; }

        public List<Item> AllItems { get; set; }
    }
}