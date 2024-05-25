using Business.BAO.Interfaces;
using Business.BAO.Services;
using Persistence.DAL;

namespace Business.BAL;

public class BusinessFacade
{
    public static BusinessFacade Instance => new();
    public IAuth AuthService { get; }
    private BusinessFacade()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);

        AuthService = new AuthService();
    }
}