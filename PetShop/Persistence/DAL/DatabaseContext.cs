using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Persistence.Entity;

namespace Persistence.DAL;

/// <summary>
/// Nested class for the database context, used to interact with the database.
/// </summary>
internal sealed class DatabaseContext : DbContext
{
    //private readonly ILogger _logger = Logger.Logger.Instance.GetLogger<DatabaseContext>();
    private readonly IntegrationMode _integrationMode;

    /// <summary>
    /// Private constructor to initialize the database context.
    /// </summary>
    public DatabaseContext(IntegrationMode integrationMode)
    {
        _integrationMode = integrationMode;
        Console.WriteLine("DatabaseContext instantiated.");
        if (_integrationMode == IntegrationMode.Integration)
            Database.EnsureDeleted();
        if (Database.EnsureCreated())
        {
            Console.WriteLine("Database created successfully.");
        }
    }

    /// <summary>
    /// DbSet for users.
    /// </summary>
    public DbSet<User> Users { get; init; }

    /// <summary>
    /// DbSet for order sessions.
    /// </summary>
    public DbSet<OrderSession> OrdersSessions { get; init; }

    /// <summary>
    /// DbSet for order products.
    /// </summary>
    public DbSet<OrderProduct> OrdersProducts { get; init; }

    /// <summary>
    /// DbSet for products.
    /// </summary>
    public DbSet<Product> Products { get; init; }

    /// <summary>
    /// DbSet for bill details.
    /// </summary>
    public DbSet<BillDetails> Bills { get; init; }

    /// <summary>
    /// Configures the database connection based on the integration mode.
    /// </summary>
    /// <param name="optionsBuilder">The options builder to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        switch (_integrationMode)
        {
            case IntegrationMode.Production:
            case IntegrationMode.Integration:
                Console.WriteLine("Integration mode: Production");
                optionsBuilder.UseSqlite($"Data Source=/home/damir/Documents/Github/PetShop-ProiectIP/PetShop/Persistence/PetShop.db");
                break;
            case IntegrationMode.Testing:
                Console.WriteLine("Integration mode: Testing");
                optionsBuilder.UseInMemoryDatabase("TestingDatabase");
                break;
            default:
                throw new CheckoutException("IntegrationMode is not set.");
        }
    }

    /// <summary>
    /// Configures the model creating process.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<OrderProduct>().ToTable("OrderProducts");
        modelBuilder.Entity<OrderSession>().ToTable("OrderSessions");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<BillDetails>().ToTable("BillDetails");
    }
}