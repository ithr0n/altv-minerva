using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class Account
    {
        public Account()
        {
            Characters = new List<Character>();
        }

        [Key]
        public string SocialClubId { get; set; }

        public string Password { get; set; }

        public int AdminLevel { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastLogin { get; set; }

        public ICollection<Character> Characters { get; set; }

    }
}
