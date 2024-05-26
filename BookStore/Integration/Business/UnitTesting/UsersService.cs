using System.Runtime.CompilerServices;
using Business.BAL;
using Business.BAO;
using Business.Mappers;
using Persistence.DAL;
using Persistence.DTO.User;

namespace UnitTesting.Business.UnitTesting;

public class UsersService
{
    private BusinessFacade _businessFacade;
    private UserRegisterDto _adminUser;
    private UserRegisterDto _clientUser;

    [OneTimeSetUp]
    public void PrepareDatabase()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        _businessFacade = BusinessFacade.Instance;

        _adminUser = new UserRegisterDto
        {
            FirstName = "AdminUsers", LastName = "AdminUsers", Username = "adminUsers_123",
            Password = "adminUsers_123", Email = "adminUsers@admin.com", UserType = "ADMIN"
        };
        var gdprAdmin = GdprMapper.DoUserInfoDtoGdpr(_adminUser);
        Assert.That(PersistenceFacade.Instance.UserRepository.RegisterUser(gdprAdmin).IsSuccess, Is.EqualTo(true));
        
        _clientUser = new UserRegisterDto
        {
            FirstName = "ClientUsers", LastName = "ClientUsers", Username = "clientUsers_123",
            Password = "client_123", Email = "client@client.com", UserType = "CLIENT"
        };
        
        Console.WriteLine( RuntimeHelpers.GetHashCode(PersistenceFacade.Instance));
    }
    
    
    [Test, Order(1)]
    public void RegisterUser()
    {
        var register = _businessFacade.UsersService.RegisterClient(_clientUser);
        Assert.That(register.IsSuccess, Is.EqualTo(true));
    }
    
    [Test, Order(2)]
    public void DeleteUser()
    {
        var delete = _businessFacade.UsersService.DeleteUser(_adminUser.Username,_clientUser.Username);
        Assert.That(delete.IsSuccess, Is.EqualTo(true));
    }
}