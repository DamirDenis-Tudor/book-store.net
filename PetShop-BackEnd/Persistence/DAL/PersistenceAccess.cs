using System.Runtime.CompilerServices;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAO.Interfaces;
using Persistence.DAO.Repositories;
using Persistence.Entity;

[assembly: InternalsVisibleTo("Business")]
namespace Persistence.DAL;
internal class PersistenceAccess
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
                .Replace("Presentation","Persistence")}/Database/PetShop.db");
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

    private static readonly IUserRepository UserRepo = new UserRepository(DatabaseContext.Instance);
    private static readonly IProductRepository ProductRepo = new ProductRepository(DatabaseContext.Instance);
    private static readonly IOrderRepository OrderRepo = new OrderRepository(DatabaseContext.Instance);
    private static readonly IBillRepository BillRepo = new BillRepository(DatabaseContext.Instance);
    
    public IUserRepository UserRepository => UserRepo;
    public IProductRepository ProductRepository { get; } = ProductRepo;
    public IOrderRepository OrderRepository { get; } = OrderRepo;
    public IBillRepository BillRepository { get; } = BillRepo;
}