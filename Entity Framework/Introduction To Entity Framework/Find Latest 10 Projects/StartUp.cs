using Find_Latest_10_Projects.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_Latest_10_Projects
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            Console.WriteLine(GetLatestProjects(context));
        }

        public static string GetLatestProjects(SoftUni2Context context)
        {
            var listing = context.Projects
                .Select(x=> new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                })
                .OrderBy(x=>x.Name)
                .Take(10)
                .ToList();

                var sb = new StringBuilder();

            foreach(var proj in listing)
            {
                sb.AppendLine($"{proj.Name}");
                sb.AppendLine($"{proj.Description}");
                sb.AppendLine($"{proj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }
            return sb.ToString();
        }
    }
}
