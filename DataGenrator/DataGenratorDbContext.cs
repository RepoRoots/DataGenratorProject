using Microsoft.EntityFrameworkCore;
using DbGenratorWithBogus.DbModels;
using DataModels;

namespace DbGenratorWithBogus
{
    public class DataGenratorDbContext : DbContext
    {
        public DataGenratorDbContext(DbContextOptions<DataGenratorDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderDetailId });
        }
    }
}
