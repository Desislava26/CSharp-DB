using System;
using System.Collections.Generic;

namespace Find_Employees_by_First_Name_Starting_with_Sa.Data.Models
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
