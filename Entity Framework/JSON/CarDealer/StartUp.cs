using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main()
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //context.SaveChanges();

            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\CarDealer\Datasets\suppliers.json");

            //Console.WriteLine(ImportSuppliers(context, inputJson));

            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\CarDealer\Datasets\parts.json");
            //Console.WriteLine(ImportParts(context, inputJson));

            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\CarDealer\Datasets\cars.json");
            //Console.WriteLine(ImportCars(context,inputJson));

            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\CarDealer\Datasets\customers.json");
            //Console.WriteLine(ImportCustomers(context,inputJson));

            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\CarDealer\Datasets\sales.json");
            //Console.WriteLine(ImportSales(context, inputJson));

            //Console.WriteLine(GetOrderedCustomers(context));

            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //Console.WriteLine(GetTotalSalesByCustomer(context));

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static void InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }

         public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeAutomapper();

            var DTOSupplier = JsonConvert.DeserializeObject<IEnumerable<SupplierDTO>>(inputJson);
            var suppliers = mapper.Map<IEnumerable<Supplier>>(DTOSupplier);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            //InitializeAutomapper();
            var suppliedIds = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var parts = JsonConvert
                .DeserializeObject<IEnumerable<Part>>(inputJson)
                .Where(s=> suppliedIds.Contains(s.SupplierId))
                .ToList();
           // var parts = mapper.Map<IEnumerable<Part>>(DTOparts);

            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            //InitializeAutomapper();
            var listOfCars = new List<Car>();
            var DTOcars = JsonConvert.DeserializeObject<IEnumerable<CarDTO>>(inputJson);
            foreach (var car in DTOcars)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TraveledDistance
                };

                foreach (var partId in car?.PartsId.Distinct())
                {
                    currentCar.PartsCars.Add(new PartCar
                    {
                        PartId = partId
                    });

                }
                listOfCars.Add(currentCar);
            }
            //var cars = mapper.Map<IEnumerable<Car>>(DTOcars);

            context.Cars.AddRange(listOfCars);
            context.SaveChanges();

            

            return $"Successfully imported {listOfCars.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeAutomapper();

            var DTOcustomers = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(inputJson);
            var customers = mapper.Map<IEnumerable<Customer>>(DTOcustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeAutomapper();

            var DTOsales = JsonConvert.DeserializeObject<IEnumerable<SaleDTO>>(inputJson);
            var sales = mapper.Map<IEnumerable<Sale>>(DTOsales);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";

        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers,Formatting.Indented);

            return json.ToString();
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x=>x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TravelledDistance,    
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars,Formatting.Indented);

            return json.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance
                    },
                    parts = x.PartsCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = $"{p.Part.Price:F2}"
                    })
                        .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json.ToString();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new 
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney = x.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json.ToString();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TraveledDistance = x.Car.TravelledDistance

                    },
                    customerName = x.Customer.Name,
                    discount = $"{x.Discount:F2}",
                    price = $"{x.Car.PartsCars.Sum(p => p.Part.Price):F2}",
                    priceWithDiscount = $@"{(x.Car.PartsCars.Sum(p => p.Part.Price) -
                                             x.Car.PartsCars.Sum(p => p.Part.Price) * x.Discount / 100):F2}"

                })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return json.ToString();
        }
    }
}