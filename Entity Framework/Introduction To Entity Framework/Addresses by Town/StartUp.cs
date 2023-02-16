using Addresses_by_Town.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses_by_Town
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            var result = GetAddressesByTown(context);
            Console.WriteLine(result);
        }
        public static string GetAddressesByTown(SoftUni2Context context)
        {
            var sb = new StringBuilder();

            var listing = context.Addresses
                .Select(a => new
            {
                Employees = a.Employees.Count,
                a.AddressText,
                TownName = a.Town.Name
            })
                .OrderByDescending(x=> x.Employees)
                .ThenBy(x=>x.TownName)
                .ThenBy(x=> x.AddressText)
                .Take(10)
                .ToList();

           
            foreach (var address in listing)
            {
               
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Employees} employees");
            }

            return sb.ToString();
        }
    }
}
