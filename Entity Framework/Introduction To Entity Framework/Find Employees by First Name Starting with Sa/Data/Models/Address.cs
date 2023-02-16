using System;
using System.Collections.Generic;

namespace Find_Employees_by_First_Name_Starting_with_Sa.Data.Models
{
    public partial class Address
    {
        public Address()
        {
            Employees = new HashSet<Employee>();
        }

        public int AddressId { get; set; }
        public string AddressText { get; set; } = null!;
        public int? TownId { get; set; }

        public virtual Town? Town { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
