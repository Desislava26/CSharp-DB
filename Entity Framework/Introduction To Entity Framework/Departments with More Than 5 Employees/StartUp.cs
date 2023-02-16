using Departments_with_More_Than_5_Employees.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments_with_More_Than_5_Employees
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUni2Context context)
        {
            var list = context.Departments
                .Select(x =>
                new
                {
                    x.Name,
                    x.Manager.FirstName,
                    x.Manager.LastName,
                    count = x.Employees.Count,
                    x.Employees
                })
                .Where(x=>x.count >5)
                .OrderBy(x=>x.count)
                .ThenBy(x=>x.Name)
                .ToList();
            var sb = new StringBuilder();

            foreach (var dep in list)
            {
                sb.AppendLine($"{dep.Name} - {dep.FirstName} {dep.LastName}");
                foreach (var emp in dep.Employees.OrderBy(x=> x.FirstName).ThenBy(x=>x.LastName))
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }
            return sb.ToString();
        }
    }
}
