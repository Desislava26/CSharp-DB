using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.DTOs.Export;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.Data.SqlClient.Server;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main()
        {
            
            var context = new CarDealerContext();
            // context.Database.EnsureDeleted();
            // context.Database.EnsureCreated();

            //var supplierXml = File.ReadAllText(@"C:\Users\Lenovo\source\repos\XML\CarDealer\Datasets\suppliers.xml");
            // Console.WriteLine(ImportSuppliers(context, supplierXml));
            //var partXml = File.ReadAllText(@"C:\Users\Lenovo\source\repos\XML\CarDealer\Datasets\parts.xml");
            //Console.WriteLine(ImportParts(context, partXml));
            //var carXml = File.ReadAllText(@"C:\Users\Lenovo\source\repos\XML\CarDealer\Datasets\cars.xml");
            //Console.WriteLine(ImportCars(context, carXml));
            //var customersXml = File.ReadAllText(@"C:\Users\Lenovo\source\repos\XML\CarDealer\Datasets\customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXml));
            //var salesXml = File.ReadAllText(@"C:\Users\Lenovo\source\repos\XML\CarDealer\Datasets\sales.xml");
            //Console.WriteLine(ImportSales(context, salesXml));

            //Console.WriteLine(GetCarsWithDistance(context));
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        private static void InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(SupplierDTO[]),
                new XmlRootAttribute("Suppliers"));

            var reader = new StringReader(inputXml);

            var suppliersDto = xmlSerializer
                .Deserialize(reader) as SupplierDTO[];

            var suppliers = suppliersDto.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter,
            });

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        { 
            var xmlSerilizer = new XmlSerializer(typeof(PartDTO[]), new XmlRootAttribute("Parts"));

            var reader = new StringReader(inputXml);

            var partsDto = xmlSerilizer.Deserialize(reader) as PartDTO[];

            var suppliersId = context.Suppliers.Select(x => x.Id).ToList();
            var parts = partsDto.Select(x => new Part
            {
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                SupplierId = x.SupplierId,
            })
                .Where(s=> suppliersId.Contains(s.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerilizer = new XmlSerializer(typeof(CarDTO[]), new XmlRootAttribute("Cars"));

            var reader = new StringReader(inputXml);

            var carsDto = xmlSerilizer.Deserialize(reader) as CarDTO[];

            var allParts = context.Parts
                .Select(x => x.Id)
                .ToList();

            var cars = carsDto.Select(x => new
            {
                Make = x.Make,
                Model = x.Model,
                TraveledDistance = x.TraveledDistance,
                PartCars = x.Parts.Select(x => x.Id)
                .Distinct()
                .Intersect(allParts)
                .Select(pc => new PartCar
                {
                    PartId = pc
                })
                .ToList()
            }).ToList();

            var carlisting = cars.Select(x=> new Car
            {
                Make = x.Make,
                Model = x.Model,
                TraveledDistance = x.TraveledDistance,
            })
                .ToList();
            context.Cars.AddRange(carlisting);
            context.SaveChanges();

            return $"Successfully imported {carlisting.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            InitializeAutomapper();
            var customerXml = new XmlSerializer(typeof(CustomerDTO[]),new XmlRootAttribute("Customers"));
            var reader = new StringReader(inputXml);
            var customerDTO = customerXml.Deserialize(reader) as CustomerDTO[];
            
            var customers = mapper.Map<Customer[]>(customerDTO);

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}";
        }


        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesXml = new XmlSerializer(typeof(SaleDTO[]), new XmlRootAttribute("Sales"));
            var reader = new StringReader(inputXml);
            var salesDTO = salesXml.Deserialize(reader) as SaleDTO[];

            var carsIds = context.Cars.Select(x => x.Id).ToList();

            var sales = salesDTO
                .Where(c => carsIds.Contains(c.CarId))
                .Select(x=> new Sale
            {
                CarId = x.CarId,
                CustomerId = x.CustomerId,
                Discount = x.Discount,
            })
                
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }


        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x=>new CarOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                })
                .Where(d=>d.TraveledDistance > 2000000)
                .OrderBy(m=>m.Make)
                .ThenBy(m=>m.Model)
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarOutputModel[]), new XmlRootAttribute("cars"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, cars, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(d => d.Make == "BMW")
                .Select(x => new BmwOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                })
                
                .OrderBy(m => m.Model)
                .ThenByDescending(m => m.TraveledDistance)
                .ToArray();


            var serializer = new XmlSerializer(typeof(BmwOutputModel[]), new XmlRootAttribute("cars"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, cars, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new SupplierOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartCount = x.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SupplierOutputModel[]), new XmlRootAttribute("suppliers"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, suppliers, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new CarPartOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TraveledDistance,
                    Parts = x.PartsCars.Select(p => new CarPartInfoModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CarPartOutputModel[]), new XmlRootAttribute("cars"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, cars, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new CustomerOutputModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpendMoney = x.Sales
                    .Select(x => x.Car)
                    .SelectMany(x => x.PartsCars)
                    .Sum(x => x.Part.Price)
                })
                .OrderByDescending(x=>x.SpendMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CustomerOutputModel[]), new XmlRootAttribute("cars"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, customers, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new SaleOutputModel
                {
                    Car = new CarSaleOutputModel
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TraveledDistance
                    },

                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartsCars.Sum(x=>x.Part.Price),
                    PriceWithDiscount =(x.Car.PartsCars.Sum(x => x.Part.Price)) - 
                    (x.Car.PartsCars.Sum(x => x.Part.Price) *x.Discount/100)
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SaleOutputModel[]), new XmlRootAttribute("sale"));
            var textWriter = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(textWriter, sales, ns);
            var result = textWriter.ToString();

            XmlDocument xmlDoc = new XmlDocument();
            StringWriter sw = new StringWriter();
            xmlDoc.LoadXml(result);
            xmlDoc.Save(sw);
            String formattedXml = sw.ToString();
            return formattedXml;

        }
    }
    
}