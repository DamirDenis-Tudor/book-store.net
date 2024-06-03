using System.ComponentModel.Design;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Entity;

namespace Persistence.DAL;

/// <summary>
/// Nested class for the database context, used to interact with the database.
/// </summary>
internal sealed class DatabaseContext : DbContext
{
    private readonly ILogger _logger = Logger.Instance.GetLogger<DatabaseContext>();
    private readonly IntegrationMode _integrationMode;

    /// <summary>
    /// Private constructor to initialize the database context.
    /// </summary>
    public DatabaseContext(IntegrationMode integrationMode)
    {
        _logger.LogInformation("DatabaseContext instantiated.");
        
        _integrationMode = integrationMode;

        if (_integrationMode == IntegrationMode.Integration)
            Database.EnsureDeleted();
        if (Database.EnsureCreated())
        {
            _logger.LogInformation("Database created successfully.");
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
    public DbSet<OrderProduct> OrderProducts { get; init; }
    
    /// <summary>
    /// DbSet for order products.
    /// </summary>
    public DbSet<ProductInfo> ProductInfos { get; init; }

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
                _logger.LogInformation("Integration mode: Production");
                optionsBuilder.UseSqlite($"Data Source={SlnDirectory.GetPath()}/Persistence/BookStore.db".Replace('/', Path.DirectorySeparatorChar));
                break;
            case IntegrationMode.Testing:
                _logger.LogInformation("Integration mode: Testing");
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
        modelBuilder.Entity<ProductInfo>().ToTable("ProductInfo");
    }
}