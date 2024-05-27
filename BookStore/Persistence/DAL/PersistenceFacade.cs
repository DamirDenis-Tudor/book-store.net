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

using Persistence.DAO.Interfaces;
using Persistence.DAO.Repositories;

namespace Persistence.DAL
{
    /// <summary>
    /// PersistenceFacade class provides a singleton facade for accessing the persistence layer.
    /// </summary>
    public class PersistenceFacade
    {
        /// <summary>
        /// Lazy initialization of the singleton instance of PersistenceFacade.
        /// </summary>
        private static readonly Lazy<PersistenceFacade> LazyInstance = new(() => new PersistenceFacade());

        /// <summary>
        /// Public accessor for the singleton instance of PersistenceFacade.
        /// </summary>
        public static PersistenceFacade Instance => LazyInstance.Value;

        /// <summary>
        /// Database context for accessing the database. Initially null.
        /// </summary>
        private DatabaseContext? _databaseContext;

        /// <summary>
        /// Repositories for managing user data.
        /// </summary>
        public IUserRepository UserRepository { get; private set; } = null!;

        /// <summary>
        /// Repositories for managing product data.
        /// </summary>
        public IProductRepository ProductRepository { get; private set; } = null!;

        /// <summary>
        /// Repositories for managing order data.
        /// </summary>
        public IOrderRepository OrderRepository { get; private set; } = null!;

        /// <summary>
        /// Repositories for managing bill data.
        /// </summary>
        public IBillRepository BillRepository { get; private set; } = null!;

        /// <summary>
        /// Private constructor to prevent instantiation from outside (singleton pattern).
        /// </summary>
        private PersistenceFacade() { }

        /// <summary>
        /// Sets the integration mode and initializes the database context and repositories.
        /// </summary>
        /// <param name="integrationMode">The integration mode to set.</param>
        public void SetIntegrationMode(IntegrationMode integrationMode)
        {
            if (_databaseContext != null) return;
            
            _databaseContext = new DatabaseContext(integrationMode);
            
            UserRepository = new UserRepository(_databaseContext);
            ProductRepository = new ProductRepository(_databaseContext);
            OrderRepository = new OrderRepository(_databaseContext);
            BillRepository = new BillRepository(_databaseContext);

        }
    }
}
