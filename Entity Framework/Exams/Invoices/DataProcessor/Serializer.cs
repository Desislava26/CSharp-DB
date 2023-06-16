namespace Invoices.DataProcessor
{
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using System.Xml;
    using Invoices.DataProcessor.ImportDto;
    using AutoMapper.QueryableExtensions;
    using System.Text;
    using Invoices.DataProcessor.ExportDto;
    using AutoMapper;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var sb = new StringBuilder();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InvoicesProfile>();
            });

            var xmlSerializer = new XmlSerializer(typeof(ClientInvoiceExportDto[]), new XmlRootAttribute("Clients"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            var clientsDtos = context
                .Clients
                .Where(c => c.Invoices.Any(ci => ci.IssueDate >= date))
                .ProjectTo<ClientInvoiceExportDto>(config)
                .ToArray();
            xmlSerializer.Serialize(sw, clientsDtos, namespaces);
            return sb.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context
                .Products
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .ToArray()
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                        .Where(pc => pc.Client.Name.Length >= nameLength)
                        .ToArray()
                        .OrderBy(pc => pc.Client.Name)
                        .Select(pc => new
                        {
                            Name = pc.Client.Name,
                            NumberVat = pc.Client.NumberVat,
                        })
                        .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            var result = JsonConvert.SerializeObject(products);
            return result.ToString();
        }
    }
}