/**************************************************************************
 *                                                                        *
 *  Description: Persistence Layer Facade                                 *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAO.Interfaces;
using Persistence.DAO.Repositories;
using Persistence.Entity;

namespace Persistence.DAL;

/// <summary>
/// Class for managing database access and repository initialization.
/// </summary>
public class PersistenceAccess
{
    /// <summary>
    /// Nested class for the database context, used to interact with the database.
    /// </summary>
    internal sealed class DatabaseContext : DbContext
    {
        private readonly ILogger _logger = Logger.Logger.Instance.GetLogger<DatabaseContext>();

        /// <summary>
        /// Private constructor to initialize the database context.
        /// </summary>
        private DatabaseContext()
        {
            _logger.LogInformation("DatabaseContext instantiated.");

            if (Database.EnsureCreated())
            {
                Console.WriteLine("Database created successfully.");
            }
        }

        /// <summary>
        /// Singleton instance of the database context.
        /// </summary>
        public static DatabaseContext Instance { get; } = new();

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

        internal static IntegrationMode IntegrationMode = IntegrationMode.NotSet;
    }

    /// <summary>
    /// Repository for user-related operations.
    /// </summary>
    public static IUserRepository UserRepository { get; private set; } = null!;

    /// <summary>
    /// Repository for product-related operations.
    /// </summary>
    public static IProductRepository ProductRepository { get; private set; } = null!;

    /// <summary>
    /// Repository for order-related operations.
    /// </summary>
    public static IOrderRepository OrderRepository { get; private set; } = null!;

    /// <summary>
    /// Repository for bill-related operations.
    /// </summary>
    public static IBillRepository BillRepository { get; private set; } = null!;

    /// <summary>
    /// Sets the integration mode for the database context and initializes repositories.
    /// </summary>
    /// <param name="integrationMode">The integration mode to set.</param>
    public static void SetIntegrationMode(IntegrationMode integrationMode)
    {
        DatabaseContext.IntegrationMode = integrationMode;
            
        UserRepository = new UserRepository(DatabaseContext.Instance);
        ProductRepository = new ProductRepository(DatabaseContext.Instance);
        OrderRepository = new OrderRepository(DatabaseContext.Instance);
        BillRepository = new BillRepository(DatabaseContext.Instance);
    } 
}