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
        private readonly ILogger _logger = Logging.Instance.GetLogger<DatabaseContext>();

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
        public DbSet<OrderProduct> Orders { get; init; }
        public DbSet<Product> Products { get; init; }
        public DbSet<BillDetails> Bills { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()
                .Replace("Presentation", "Persistence")}/Database/PetShop.db");
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

    public IUserRepository UserRepository { get; } = new UserRepository(DatabaseContext.Instance);
    public IProductRepository ProductRepository { get; } = new ProductRepository(DatabaseContext.Instance);
    public IOrderRepository OrderRepository { get; } = new OrderRepository(DatabaseContext.Instance);
    public IBillRepository BillRepository { get; } = new BillRepository(DatabaseContext.Instance);
}