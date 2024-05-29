using Business.BAL;
using Business.BAO;
using Business.BTO;
using Persistence.DAL;
using Persistence.DTO.User;

namespace UnitTesting.Business.UnitTesting;

public class AuthService
{
    private BusinessFacade _businessFacade;
    private UserRegisterDto _user;

    [OneTimeSetUp]
    public void PrepareDatabase()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        _businessFacade = BusinessFacade.Instance;
        _user = new UserRegisterDto
        {
            FirstName = "Test", LastName = "Test", Username = "test_123",
            Password = "test_123", Email = "test@test.com", UserType = "CLIENT"
        };
    }

    [Test, Order(1)]
    public void LoginTest()
    {
        var userLoginBto = new UserLoginBto
        {
            Username = _user.Username,
            Password = _user.Password
        };

        Assert.That(_businessFacade.AuthService.Login(userLoginBto, LoginMode.Client).ErrorType,
            Is.EqualTo(BaoErrorType.UserPasswordNotFound));
    }

    [Test, Order(2)]
    public void Register()
    {
        Assert.That(_businessFacade.UsersService.RegisterClient(_user).IsSuccess,
            Is.EqualTo(true));
    }

    [Test, Order(3)]
    public void CheckSessionAndLogout()
    {
        var userLoginBto = new UserLoginBto
        {
            Username = _user.Username,
            Password = _user.Password
        };

        var login = _businessFacade.AuthService.Login(userLoginBto, LoginMode.Client);
        Assert.That(login.IsSuccess, Is.EqualTo(true));

        Assert.That(_businessFacade.AuthService.CheckSession(login.SuccessValue).IsSuccess,
            Is.EqualTo(true));

        var getUsername = _businessFacade.AuthService.GetUsername(login.SuccessValue);
        Assert.That(getUsername.IsSuccess, Is.EqualTo(true));
        
        Assert.That(_businessFacade.AuthService.Logout(login.SuccessValue).IsSuccess, Is.EqualTo(true));
    }
}