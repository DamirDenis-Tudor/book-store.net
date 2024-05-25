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

namespace Persistence.DAL;

public class PersistenceFacade
{
    private static readonly Lazy<PersistenceFacade> LazyInstance = new(() => new PersistenceFacade());

    public static PersistenceFacade Instance => LazyInstance.Value;

    private DatabaseContext? _databaseContext;
    public IUserRepository UserRepository { get; private set; } = null!;
    public IProductRepository ProductRepository { get; private set; } = null!;
    public IOrderRepository OrderRepository { get; private set; } = null!;
    public IBillRepository BillRepository { get; private set; } = null!;

    private PersistenceFacade() { }

    public void SetIntegrationMode(IntegrationMode integrationMode)
    {
        _databaseContext = new DatabaseContext(integrationMode);
        
        UserRepository = new UserRepository(_databaseContext);
        ProductRepository = new ProductRepository(_databaseContext);
        OrderRepository = new OrderRepository(_databaseContext);
        BillRepository = new BillRepository(_databaseContext);
    }
}
