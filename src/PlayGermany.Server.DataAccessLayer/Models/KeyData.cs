using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class KeyData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Key { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}