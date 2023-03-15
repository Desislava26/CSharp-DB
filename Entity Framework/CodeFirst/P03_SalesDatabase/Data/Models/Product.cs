using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }
        public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}
