using Microsoft.EntityFrameworkCore;
using Remove_Town.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remove_Town
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
        }

        public static string RemoveTown(SoftUni2Context context)
        {
            var townForDelete = "Seattle";

            var town = context.Towns.FirstOrDefault(t => t.Name == townForDelete);
            var addresses = context
                .Addresses
                .Where(a => a.TownId == town.TownId)
                .ToList();

            foreach (var emp in context.Employees)
            {
                if (addresses.Contains(emp.Address))
                {
                    emp.Address = null;
                }
            }
            int count = addresses.Count();
            
            context.Addresses.RemoveRange(addresses);
            context.Towns.Remove(town);
            context.SaveChanges();


            return $"{count}";
        }
    }
}
