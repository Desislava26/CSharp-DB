using System;
using System.Collections.Generic;

namespace Employees_from_Research_and_Development.Data.Models
{
    public partial class Project
    {
        public Project()
        {
            Employees = new HashSet<Employee>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
