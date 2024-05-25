using Business.BAO.Interfaces;
using Business.BAO.Services;
using Persistence.DAL;

namespace Business.BAL;

public class BusinessFacade
{
    public static BusinessFacade Instance => new();
    public IAuthentication AuthenticationService { get; }
    private BusinessFacade()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);

        AuthenticationService = new AuthenticationService();
    }
}