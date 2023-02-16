using Employees_and_Projects.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees_and_Projects
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(GetEmployeesInPeriod(context));
        }
        public static string GetEmployeesInPeriod(SoftUni2Context context)
        {
            var list = context.Employees
                .Include(x => x.Projects)
                .Where(x=>x.Projects.Any(p=>p.StartDate.Year >=2001 && p.StartDate.Year <=2003))
                .Take(10)
                .Select(x=> new
                {
                   empFirstName= x.FirstName,
                   empLastName =  x.LastName,
                   managerFirstName= x.Manager.FirstName,
                    manegerLastName = x.Manager.LastName,
                  projects = x.Projects.Select(p => new
                   {
                      projectName = p.Name,
                      startDate = p.StartDate,
                      endDate = p.EndDate
                   })
                })
                .ToList();
            var sb = new StringBuilder();
            foreach (var emp in list)
            {
                sb.AppendLine($"{emp.empFirstName} {emp.empLastName} - Manager: {emp.managerFirstName} {emp.manegerLastName}");
                foreach (var project in emp.projects)
                {
                    var end = project.endDate;
                    if (project.endDate.HasValue)
                    {
                        sb.AppendLine($"--{project.projectName} - {project.startDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {((DateTime)project.endDate).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        sb.AppendLine($"--{project.projectName} - {project.startDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - not finished");
                    }
                   
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}
