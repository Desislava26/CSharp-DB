using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        public string Name { get; set; }

        //collection
        public ICollection<Team> PrimaryKitTeams { get; set; } = new HashSet<Team>();
        public ICollection<Team> SecondaryKitTeams { get; set; } = new HashSet<Team>();
    }
}
