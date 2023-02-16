using Employees_from_Research_and_Development.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees_from_Research_and_Development
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
           var result = GetEmployeesFromResearchAndDevelopment(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUni2Context context)
        {
            var list = context.Employees
                .Select(x=>
                new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department,
                    x.Salary
                })
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in list)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} from {emp.Department.Name} - ${emp.Salary:f2}");
            }
            return sb.ToString();
        }
    }
}
