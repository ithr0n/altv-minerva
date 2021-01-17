using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minerva.Server.Core.Contracts.Models
{
    public class KeyChain
    {
        public KeyChain()
        {
            Keys = new List<KeyData>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public List<KeyData> Keys { get; set; }
    }
}