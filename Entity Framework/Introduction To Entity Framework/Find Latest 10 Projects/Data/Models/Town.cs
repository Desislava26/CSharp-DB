using System;
using System.Collections.Generic;

namespace Find_Latest_10_Projects.Data.Models
{
    public partial class Town
    {
        public Town()
        {
            Addresses = new HashSet<Address>();
        }

        public int TownId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
