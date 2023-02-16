using Increase_Salaries.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Increase_Salaries
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(IncreaseSalaries(context));
        }

        public static string IncreaseSalaries(SoftUni2Context context)
        {
            var departments = new string[]
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };
            var listing = context.Employees
                .Where(x=>departments.Contains(x.Department.Name))
                .ToList();

            var sb = new StringBuilder();

            var list = context.Employees.ToList();
            foreach (var emp in listing.OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName))
            {
                emp.Salary *= 1.12m;
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString();
        }

    }
}
