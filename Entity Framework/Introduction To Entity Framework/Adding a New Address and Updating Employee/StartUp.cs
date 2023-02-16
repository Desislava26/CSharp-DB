using Adding_a_New_Address_and_Updating_Employee.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adding_a_New_Address_and_Updating_Employee
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUni2Context();
            var result = AddNewAddressToEmployee(context);
            Console.WriteLine(result);
        }

        public static string AddNewAddressToEmployee(SoftUni2Context context)
        {
            var adress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4,

            };
            //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Addresses] ON");

            context.Addresses.Add(adress);
             //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Addresses ON");

            context.SaveChanges();

            var nakov = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            nakov.AddressId = adress.AddressId;
            context.SaveChanges();

            var list = context.Employees
                .Select(x =>
                new
                {
                    x.Address.AddressText,
                    x.AddressId
                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in list)
            {
                sb.AppendLine($"{emp.AddressText}");
            }
            return sb.ToString();
        }
    }
}
