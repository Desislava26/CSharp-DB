using Employee_147.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Employee_147
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            var result = GetEmployee147(context);
            Console.WriteLine(result);
        }
        public static string GetEmployee147(SoftUni2Context context)
        {
            var obj = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Projects
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var emp in obj)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                foreach (var proj in emp.Projects.OrderBy(x=> x.Name))
                {
                    sb.AppendLine($"{proj.Name}");
                }
            }
            return sb.ToString();
        }
    }
}
