using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;


namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main()
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();


            //var inputJson = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\ProductShop\Datasets\users.json");

            //ImportUsers(context, inputJson);

            //var inputJsonProduct = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\ProductShop\Datasets\products.json");
            //ImportProducts(context, inputJsonProduct);

            //var inputJsonCategory = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\ProductShop\Datasets\categories.json");
            //ImportCategories(context, inputJsonCategory);

            //var inputJsonCategoryProducts = File.ReadAllText(@"C:\Users\Lenovo\source\repos\JSON\ProductShop\Datasets\categories-products.json");
            //ImportCategoryProducts(context, inputJsonCategoryProducts);

            // Console.WriteLine(GetProductsInRange(context));

            // Console.WriteLine(GetSoldProducts(context));

            // Console.WriteLine(GetCategoriesByProductsCount(context));

            Console.WriteLine(GetUsersWithProducts(context));

            ;
        }

        public static void InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = config.CreateMapper();
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();
            var DTOusers = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(inputJson);
            var users = mapper.Map<IEnumerable<User>>(DTOusers);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJsonProduct)
        {
            InitializeAutomapper();

            var DTOproducts = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(inputJsonProduct);
            var products = mapper.Map<IEnumerable<Product>>(DTOproducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJsonCategory)
        {
            InitializeAutomapper();

            var DTOCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(inputJsonCategory)
                .Where(x => x.Name != null)
                .ToList();
            var categories = mapper.Map<IEnumerable<Category>>(DTOCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJsonCategoryProducts)
        {
            InitializeAutomapper();

            var DTOCategoryProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProductDTO>>(inputJsonCategoryProducts);
            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(DTOCategoryProducts);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);
            return result.ToString();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(x => x.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Where(x => x.BuyerId != null)
                    .Select(y => new
                    {
                        name = y.Name,
                        price = y.Price,
                        buyerFirstName = y.Buyer.FirstName,
                        buyerLastName = y.Buyer.LastName
                    })
                    .ToList()
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList()
                ;

            var result = JsonConvert.SerializeObject(users, Formatting.Indented);
            return result.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoriesProducts.Count(),
                    averagePrice = $"{x.CategoriesProducts.Select(x => x.Product.Price).Sum() / x.CategoriesProducts.Count():f2}",
                    totalRevenue = $"{x.CategoriesProducts.Select(x => x.Product.Price).Sum():f2}"
                })
            .OrderByDescending(x => x.productsCount)
            .ToList();

            var result = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return result.ToString();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x=>x.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(x=>x.BuyerId !=null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(x => x.BuyerId != null).Count(),
                        products = u.ProductsSold.Where(x=>x.BuyerId !=null).Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                })
                .OrderByDescending(x=>x.soldProducts.products.Count())
                .ToList();
            var resultObject = new
            {
                usersCount = users.Count(),
                users = users
            };
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            var result = JsonConvert.SerializeObject(resultObject, Formatting.Indented, jsonSettings);
            return result;
        }
    }
}
