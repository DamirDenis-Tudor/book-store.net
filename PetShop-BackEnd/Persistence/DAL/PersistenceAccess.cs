using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Persistence.DAO.Interfaces;
using Persistence.DAO.Repositories;
using Persistence.Entity;

[assembly: InternalsVisibleTo("Business")]
namespace Persistence.DAL;
internal class PersistenceAccess
{
    internal sealed class DatabaseContext : DbContext
    {
        private DatabaseContext()
        {
            Console.WriteLine("DatabaseContext instantiated.");
            if (Database.EnsureCreated())
            {
                Console.WriteLine("Successfully created.");
            }
        }

        public static DatabaseContext Instance { get; } = new();

        public DbSet<User> Users { get; init; }
        public DbSet<Order> Orders { get; init; }
        public DbSet<Product> Products { get; init; }
        public DbSet<Bill> Bills { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()
                .Replace("Presentation","Persistence")}/Database/PetShop.db");
        }
            

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Bill>().ToTable("BillDetails");
        }
    }

    private static readonly IUserRepository UserRepo = new UserRepository(DatabaseContext.Instance);
    private static readonly IProductRepository ProductRepo = new ProductRepository(DatabaseContext.Instance);
    private static readonly IOrderRepository OrderRepo = new OrderRepository(DatabaseContext.Instance);
    private static readonly IDeliveryRepository DeliveryRepo = new DeliveryRepository(DatabaseContext.Instance);
    
    public IUserRepository UserRepository => UserRepo;
    public IProductRepository ProductRepository { get; } = ProductRepo;
    public IOrderRepository OrderRepository { get; } = OrderRepo;
    public IDeliveryRepository DeliveryRepository { get; } = DeliveryRepo;
}