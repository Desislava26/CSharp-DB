using Employees_with_Salary_Over_50_000.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees_with_Salary_Over_50_000
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            var result = GetEmployeesWithSalaryOver50000(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUni2Context context)
        {
            var list = context.Employees
                .Select(x =>
                new
                {
                    x.FirstName,
                    x.Salary
                })
                .Where(x=> x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .ToList();
            var sb = new StringBuilder();
            foreach(var emp in list)
            {
                sb.AppendLine($"{emp.FirstName} - {emp.Salary:f2}");
            }
            return sb.ToString();
        }
    }
}
