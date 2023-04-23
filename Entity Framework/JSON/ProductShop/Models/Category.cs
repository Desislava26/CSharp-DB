namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;

    public class Category
    {
        public Category()
        {
            CategoriesProducts = new List<CategoryProduct>();
        }

        public int Id { get; set; }
        [AllowNull]
        public string? Name { get; set; } = null!;

        public ICollection<CategoryProduct> CategoriesProducts { get; set; }
    }
}
