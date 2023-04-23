using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using ProductShop.Models;
using System.Security.Cryptography.X509Certificates;

namespace ProductShop.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
        {
        }

        public ProductShopContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CategoryProduct> CategoriesProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(x => new { x.CategoryId, x.ProductId });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(x => x.ProductsBought)
                      .WithOne(x => x.Buyer)
                      .HasForeignKey(x => x.BuyerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(x => x.ProductsSold)
                      .WithOne(x => x.Seller)
                      .HasForeignKey(x => x.SellerId)
                      .OnDelete(DeleteBehavior.NoAction);
                      

                
            });

            

        }
    }
}
// Microsoft.Data.SqlClient.SqlException:
// 'Introducing FOREIGN KEY constraint 'FK_Products_Users_SellerId'
// on table 'Products' may cause cycles or multiple cascade paths.
// Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
//Could not create constraint or index. See previous errors.'


//SqlException: The MERGE statement conflicted with the FOREIGN KEY constraint
//"FK_Products_Users_BuyerId". The conflict occurred in database "ProductShop",
//table "dbo.Users", column 'Id'.
//The statement has been terminated.