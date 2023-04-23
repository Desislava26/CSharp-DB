using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductShop.DTOs
{
    public class CategoryDTO
    {
        [AllowNull]
        public string? Name { get; set; }
    }
}
