namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        static IMapper mapper;
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(CLientDTO[]),
                new XmlRootAttribute("Clients"));

            var reader = new StringReader(xmlString);

            var clientsDto = xmlSerializer
                .Deserialize(reader) as CLientDTO[];
            var sb = new StringBuilder();
            var clients = new List<Client>();


            foreach (var clientDto in clientsDto)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client c = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat
                };

                foreach (var addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address a = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country
                    };

                    c.Addresses.Add(a);
                }
                clients.Add(c);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, c.Name));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            InitializeAutomapper();
            var DTOinvoices = JsonConvert.DeserializeObject<IEnumerable<InvoiceDTO>>(jsonString);

            var sb = new StringBuilder();
            var listing = new List<Invoice>();

            foreach (var item in DTOinvoices)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (item.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || item.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice()
                {
                    Number = item.Number,
                    IssueDate = item.IssueDate,
                    DueDate = item.DueDate,
                    Amount = item.Amount,
                    CurrencyType = item.CurrencyType,
                    ClientId = item.ClientId,

                };

                if (invoice.IssueDate > invoice.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                listing.Add(invoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, item.Number));

            }
            var invoices = mapper.Map<IEnumerable<Invoice>>(listing);

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            return sb.ToString();

        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            InitializeAutomapper();
            var DTOproducts = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(jsonString);

            var sb = new StringBuilder();
            var listing = new List<Product>();

            foreach (var item in DTOproducts)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product()
                {
                    Name = item.Name,
                    Price = item.Price,
                    CategoryType = item.CategoryType
                };

                foreach (var id in product.ProductsClients)
                {
                    var c = context.Clients.Find(id);
                    if (c==null)
                    {
                        sb.AppendLine(ErrorMessage);
                    }

                    product.ProductsClients.Add(new ProductClient()
                    {
                        Client = c
                    });
                }
                listing.Add(product);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));

            }

            context.Products.AddRange(listing);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        public static void InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile<InvoicesProfile>();
            });
            mapper = config.CreateMapper();
        }
    } 
}
