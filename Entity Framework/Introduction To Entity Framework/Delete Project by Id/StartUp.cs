using Delete_Project_by_Id.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delete_Project_by_Id
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(DeleteProjectById(context));
        }
        public static string DeleteProjectById(SoftUni2Context context)
        {
            var sb = new StringBuilder();

            var forDelete = 2;

            Project project = context.Projects.Find(forDelete);

            var employees = context.Employees
                .Where(x => x.Projects.Any(x => x.ProjectId == 2))
               .ToList();


            context.EmployeesProjects.RemoveRange(employees);
            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context
                .Projects
                .Select(p => new
                {
                    p.Name
                })
                .Take(10)
                .ToList();

            foreach (var pr in projects)
            {
                sb.AppendLine($"{pr.Name}");
            }

            return sb.ToString();
        }
    }
}
