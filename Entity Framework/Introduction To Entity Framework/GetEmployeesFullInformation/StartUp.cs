using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Models;

namespace Test
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            var result = GetEmployeesFullInformation(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUni2Context context)
        {
            var list = context.Employees
                .Select(x => new {
                x.EmployeeId,
                x.FirstName,
                x.MiddleName,
                x.LastName,
                x.JobTitle,
                x.Salary})
                .OrderBy(x=> x.EmployeeId)
                .ToList();
            var sb = new StringBuilder();
            foreach(var emp in list)
            {
                sb.AppendLine($"{emp.FirstName} {emp.MiddleName} {emp.LastName} {emp.JobTitle} {emp.Salary:f2}");
            }
            return sb.ToString();
        }
    }
}
