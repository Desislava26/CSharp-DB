using Find_Employees_by_First_Name_Starting_with_Sa.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_Employees_by_First_Name_Starting_with_Sa
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUni2Context context)
        {
            var listing = context.Employees
                .Where(x=> x.FirstName.StartsWith("Sa"))
                .Select(x=> new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    Salary = x.Salary
                })
                .OrderBy(x=>x.FirstName)
                .ThenBy(x=>x.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in listing)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }
            return sb.ToString();
        }
    }
}
