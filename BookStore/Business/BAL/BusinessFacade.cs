/**************************************************************************
 *                                                                        *
 *  Description: Business Layer Facade                                    *
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

using System.Runtime.CompilerServices;
using Business.BAO.Interfaces;
using Business.BAO.Services;
using Persistence.DAL;

[assembly: InternalsVisibleTo("Integration")]
// TODO: username as hash
namespace Business.BAL
{
    /// <summary>
    /// Facade for business layer, providing a simplified interface to various business services.
    /// </summary>
    public class BusinessFacade
    {
        /// <summary>
        /// Gets the singleton instance of the BusinessFacade class.
        /// </summary>
        public static BusinessFacade Instance => new();

        /// <summary>
        /// Gets the authentication service.
        /// </summary>
        public IAuth AuthService { get; }

        /// <summary>
        /// Gets the inventory service.
        /// </summary>
        public IInventory InventoryService { get; }

        /// <summary>
        /// Gets the order service.
        /// </summary>
        public IOrder OrderService { get; }

        /// <summary>
        /// Gets the users service.
        /// </summary>
        public IUsers UsersService { get; }

        /// <summary>
        /// Private constructor to prevent direct instantiation.
        /// Initializes the persistence facade and business services.
        /// </summary>
        public BusinessFacade()
        {
            PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);
            
            AuthService = new AuthService();
            InventoryService = new InventoryService();
            OrderService = new OrderService();
            UsersService = new UserService();
        }
    }
}
