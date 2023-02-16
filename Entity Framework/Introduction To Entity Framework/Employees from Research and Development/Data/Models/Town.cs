using System;
using System.Collections.Generic;

namespace Employees_from_Research_and_Development.Data.Models
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
