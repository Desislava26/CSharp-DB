using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Seller { get; set; }
        public int SellerId { get; set; }
        [AllowNull]
        public int? BuyerId { get; set; }
    }
}
