using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bogus;
using DbGenratorWithBogus.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DbGenratorWithBogus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize data context
            string connectionString = "Server=localhost;Database=RandomData;Integrated Security=True;Encrypt=true;TrustServerCertificate=true;";

            // Configure DbContextOptionsBuilder with SQL Server connection and increased command timeout
            var contextOptionsBuilder = new DbContextOptionsBuilder<DataGenratorDbContext>();
            contextOptionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.CommandTimeout(1000); // 600 seconds = 10 minutes
            });

            // Create instance of DbContext with configured options
            var context = new DataGenratorDbContext(contextOptionsBuilder.Options);

            // Create database
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Generate data for products
            await GenerateAndInsertDataAsync<Product>(context, () => GenerateProducts(1000000), "Products", 1000000, 100000);

            // Generate data for customers
            await GenerateAndInsertDataAsync<Customer>(context, () => GenerateCustomers(1000000), "Customers", 1000000, 100000);

            // Generate data for orders
            var orders = await GenerateAndInsertDataAsync<Order>(context, () => GenerateOrders(500000), "Orders", 500000, 100000);

            // Generate data for order details
            var existingOrderIds = orders.Select(o => o.OrderId).ToList();
            await GenerateAndInsertDataAsync<OrderDetail>(context, () => GenerateOrderDetails(existingOrderIds), "OrderDetails", 2000000, 100000);

            // Generate data for users


            Console.WriteLine("Data insertion completed!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task<List<T>> GenerateAndInsertDataAsync<T>(DataGenratorDbContext context, Func<List<T>> dataGenerator, string tableName, int totalRecords, int batchSize) where T : class
        {
            Console.WriteLine($"Generating and inserting data for {tableName}...");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var data = dataGenerator();
            var batches = SplitIntoBatches(data, batchSize);

            foreach (var batch in batches)
            {
                Console.WriteLine($"Inserting batch of {batch.Count} records into {tableName}...");

                await context.AddRangeAsync(batch);
                await context.SaveChangesAsync(); // Save changes for each batch
            }

            stopwatch.Stop();
            Console.WriteLine($"Elapsed time for {tableName}: {stopwatch.Elapsed}");

            return data;
        }

        static List<List<T>> SplitIntoBatches<T>(List<T> data, int batchSize)
        {
            var batches = new List<List<T>>();
            for (int i = 0; i < data.Count; i += batchSize)
            {
                batches.Add(data.Skip(i).Take(batchSize).ToList());
            }
            return batches;
        }

        static List<Product> GenerateProducts(int count)
        {
            var productFaker = new Faker<Product>();
            return productFaker.RuleFor(p => p.Code, f => f.Commerce.Ean13())
                .RuleFor(p => p.Description, f => f.Commerce.ProductName())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                .Generate(count);
        }

        static List<Customer> GenerateCustomers(int count)
        {
            var customerFaker = new Faker<Customer>();
            return customerFaker.RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Address, f => f.Address.StreetAddress())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.StateProvince, f => f.Address.State())
                .RuleFor(c => c.Country, f => f.Address.Country())
                .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                .RuleFor(c => c.DateOfBirth, f => f.Date.Past(80))
                .Generate(count);
        }

        static List<Order> GenerateOrders(int count)
        {
            var orderFaker = new Faker<Order>();
            return orderFaker.RuleFor(o => o.CustomerId, f => f.Random.Int(1, 1000000))
                .RuleFor(o => o.OrderDate, f => f.Date.Past(5))
                .RuleFor(o => o.ShippingAddress, f => f.Address.StreetAddress())
                .RuleFor(o => o.TotalAmount, f => f.Random.Decimal(10, 10000))
                .Generate(count);
        }

        static List<OrderDetail> GenerateOrderDetails(List<int> existingOrderIds)
        {
            var orderDetailFaker = new Faker<OrderDetail>();
            return orderDetailFaker.RuleFor(od => od.OrderId, f => f.PickRandom(existingOrderIds))
                .RuleFor(od => od.ProductId, f => f.Random.Int(1, 1000000))
                .RuleFor(od => od.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(od => od.UnitPrice, f => f.Random.Decimal(1, 1000))
                .Generate(2000000);
        }


    }
}
