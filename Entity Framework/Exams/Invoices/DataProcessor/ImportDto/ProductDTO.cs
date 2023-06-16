using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(9)]
        public string? Name { get; set; }

        [Required]
        [Range(5, 1000)]
        public decimal Price { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        public ICollection<ProductClient>? ProductsClients { get; set; } = new List<ProductClient>();
    }
}
