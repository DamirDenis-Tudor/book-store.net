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
    
    private readonly IUserRepository _userRepository=  new UserRepository(DatabaseContext.Instance);
    private readonly IProductRepository _productRepository = new ProductRepository(DatabaseContext.Instance);
    private readonly IOrderRepository _orderRepository =new OrderRepository(DatabaseContext.Instance);
    private readonly IBillRepository _billRepository = new BillRepository(DatabaseContext.Instance);

    public IUserRepository UserRepository => _userRepository;
    public IProductRepository ProductRepository => _productRepository;
    public IOrderRepository OrderRepository => _orderRepository;
    public IBillRepository BillRepository => _billRepository;
}