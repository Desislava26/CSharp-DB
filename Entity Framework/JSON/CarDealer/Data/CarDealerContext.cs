using Microsoft.EntityFrameworkCore;
using CarDealer.Models;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
        {
        }

        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }
      
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartCar> PartsCars { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasMany(x => x.Parts);
            });
        }
    }
}

//SqlException: The MERGE statement conflicted with the FOREIGN KEY constraint "FK_Parts_Suppliers_SupplierId".
//    The conflict occurred in database "CarDealer", table "dbo.Suppliers", column 'Id'.
//The statement has been terminated.
