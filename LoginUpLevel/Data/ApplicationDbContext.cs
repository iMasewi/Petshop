using LoginUpLevel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LoginUpLevel.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetail { get; set; } = null!;
        public DbSet<ProductManager> ProductManagers { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<OrderAdress> OrderAdress { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<ProductColor> ProductColors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product-Employee many-to-many relationship
            modelBuilder.Entity<Product>()
                .HasMany(m => m.Employee)
                .WithMany(g => g.Products)
                .UsingEntity<ProductManager>(
                    j => j.HasOne<Employee>(mg => mg.Employee).WithMany(g => g.ProductManagers),
                    j => j.HasOne<Product>(mg => mg.Product).WithMany(m => m.ProductManager)
                );

            // Product-Order many-to-many relationship
            modelBuilder.Entity<Product>()
                .HasMany(m => m.Orders)
                .WithMany(g => g.Products)
                .UsingEntity<OrderDetail>(
                    j => j.HasOne<Order>(mg => mg.Order).WithMany(g => g.OrderItems),
                    j => j.HasOne<Product>(mg => mg.Product).WithMany(m => m.OrderDetails)
                );


            modelBuilder.Entity<Product>()
                .HasMany(m => m.Carts)
                .WithMany(g => g.Products)
                .UsingEntity<CartItem>(
                    j => j.HasOne<Cart>(mg => mg.Cart).WithMany(g => g.CartItems),
                    j => j.HasOne<Product>(mg => mg.Product).WithMany(m => m.CartItem)
                );
            modelBuilder.Entity<Product>()
                .HasMany(m => m.Colors)
                .WithMany(g => g.Product)
                .UsingEntity<ProductColor>(
                    j => j.HasOne<Color>(mg => mg.Color).WithMany(g => g.ProductColors),
                    j => j.HasOne<Product>(mg => mg.Product).WithMany(m => m.ProductColors)
                );

            // Customer-Order one-to-many relationship
            modelBuilder.Entity<Customer>()
                .HasMany(m => m.Orders)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId);

            // Status-Order one-to-many relationship
            modelBuilder.Entity<Status>()
                .HasMany(m => m.Orders)
                .WithOne(s => s.Status)
                .HasForeignKey(s => s.StatusId);
            // Customer-OrderAdress one-to-many relationship
            modelBuilder.Entity<Customer>()
                .HasMany(m => m.OrderAdress)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId);

            // Seed data cho Status
            List<Status> statuses = new List<Status>
            {
                new Status
                {
                    Id = 1,
                    StatusName = "Pending",
                    Description = "Chờ xác nhận"
                },
                new Status
                {
                    Id = 2,
                    StatusName = "Processing",
                    Description = "Đang xử lý"
                },
                new Status
                {
                    Id = 3,
                    StatusName = "Completed",
                    Description = "Hoàn thành"
                },
                new Status
                {
                    Id = 4,
                    StatusName = "Cancelled",
                    Description = "Đã hủy"
                }
            };
            modelBuilder.Entity<Status>().HasData(statuses);
            List<IdentityRole<int>> roles = new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
                },
                 new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "33333333-3333-3333-3333-333333333333"
                },
            };
            modelBuilder.Entity<IdentityRole<int>>().HasData(roles);
        }
    }
}