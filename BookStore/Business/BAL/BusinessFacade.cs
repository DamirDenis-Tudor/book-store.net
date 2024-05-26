using Business.BAO.Interfaces;
using Business.BAO.Services;
using Persistence.DAL;

namespace Business.BAL;

public class BusinessFacade
{
    public static BusinessFacade Instance => new();
    public IAuth AuthService { get; }
    public IInventory InventoryService { get; }
    public IOrder OrderService { get; }
    public IUsers UsersService { get; }
    private BusinessFacade()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);

        AuthService = new AuthService();
        InventoryService = new InventoryService();
        OrderService = new OrderService();
        UsersService = new UserService();
    }
}