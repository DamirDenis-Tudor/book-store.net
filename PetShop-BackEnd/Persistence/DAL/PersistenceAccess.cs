using System.Collections.Specialized;
using System.ComponentModel.Design;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAO.Interfaces;
using Persistence.DAO.Repositories;
using Persistence.Entity;


namespace Persistence.DAL;

public class PersistenceAccess
{
    internal sealed class DatabaseContext : DbContext
    {
        private readonly ILogger _logger = Logger.Logger.Instance.GetLogger<DatabaseContext>();

        private DatabaseContext()
        {
            _logger.LogInformation("DatabaseContext instantiated.");

            if (Database.EnsureCreated())
            {
                Console.WriteLine("Successfully created.");
            }
        }

        public static DatabaseContext Instance { get; } = new();

        public DbSet<User> Users { get; init; }
        public DbSet<OrderSession> OrdersSessions { get; init; }
        public DbSet<OrderProduct> OrdersProducts { get; init; }
        public DbSet<Product> Products { get; init; }
        public DbSet<BillDetails> Bills { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (IntegrationMode)
            {
                case IntegrationMode.Production:
                    optionsBuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}/PetShop.db");
                    break;
                case IntegrationMode.Testing:
                    optionsBuilder.UseInMemoryDatabase("TestingDatabase");
                    break;
                default:
                    throw new CheckoutException("IntegrationMode is not set.");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<OrderProduct>().ToTable("OrderProducts");
            modelBuilder.Entity<OrderSession>().ToTable("OrderSessions");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<BillDetails>().ToTable("BillDetails");
        }

        internal static IntegrationMode IntegrationMode = IntegrationMode.NotSet;
    }

    public static IUserRepository UserRepository { get; private set; } = null!;
    public static IProductRepository ProductRepository { get; private set; } = null!;
    public static IOrderRepository OrderRepository { get; private set; } = null!;
    public static IBillRepository BillRepository { get; private set; } = null!;

    public static void SetIntegrationMode(IntegrationMode integrationMode)
    {
        DatabaseContext.IntegrationMode = integrationMode;
        
        UserRepository = new UserRepository(DatabaseContext.Instance);
        ProductRepository = new ProductRepository(DatabaseContext.Instance);
        OrderRepository = new OrderRepository(DatabaseContext.Instance);
        BillRepository = new BillRepository(DatabaseContext.Instance);
    } 
}