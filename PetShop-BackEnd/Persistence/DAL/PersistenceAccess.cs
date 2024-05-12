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
            Database.Migrate();
            
            if (Database.EnsureCreated())
            {
                Console.WriteLine("Successfully created.");
            }
        }

        public static DatabaseContext Instance { get; } = new();

        public DbSet<User> Users { get; init; }
        public DbSet<OrderProduct> Orders { get; init; }
        public DbSet<Product> Products { get; init; }
        public DbSet<BillDetails> Bills { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}/PetShop.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<OrderProduct>().ToTable("Orders");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<BillDetails>().ToTable("BillDetails");
        }
    }

    public static IUserRepository UserRepository { get; } = new UserRepository(DatabaseContext.Instance);
    public static IProductRepository ProductRepository { get; } = new ProductRepository(DatabaseContext.Instance);
    public static IOrderRepository OrderRepository { get; } = new OrderRepository(DatabaseContext.Instance);
    public static IBillRepository BillRepository { get; } = new BillRepository(DatabaseContext.Instance);
}