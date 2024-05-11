using Logger;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace UnitTesting;

public class PersistenceUnit
{
    private ILogger _logger = Logging.Instance.GetLogger<PersistenceUnit>();

    [Test]
    public void CreateAndDeleteUserUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "test", LastName = "test", Username = "test_12345",
            Password = "test", Email = "test@test.test", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user), Is.EqualTo(true), "User already registered.");
        Thread.Sleep(100);
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user), Is.EqualTo(false), "User not found registered.");
        Thread.Sleep(100);
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username), Is.EqualTo(true),
            "User not found deleted.");
        Thread.Sleep(100);
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username), Is.EqualTo(true),
            "User not found deleted.");
        Thread.Sleep(100);
    }

    [Test]
    public void UpdateUserUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "test", LastName = "test", Username = "test_12345",
            Password = "test", Email = "test@test.test", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user), Is.EqualTo(true));

        var userUpdated = new UserInfoDto
        {
            FirstName = "NonTest", LastName = "", Username = "nonTest",
            Password = "", Email = "", UserType = ""
        };
        Assert.That(PersistenceAccess.UserRepository.UpdateUser(user.Username, userUpdated), Is.EqualTo(true));
        Assert.That(PersistenceAccess.UserRepository.GetUser(userUpdated.Username)?.FirstName, Is.EqualTo("NonTest"));
        
        var userUpdatedUsername = new UserInfoDto
        {
            FirstName = "test1", LastName = "", Username = "test",
            Password = "", Email = "", UserType = ""
        };
        Assert.That(PersistenceAccess.UserRepository.UpdateUser(userUpdated.Username, userUpdatedUsername), Is.EqualTo(true));
        Assert.That(PersistenceAccess.UserRepository.GetUser(user.Username), Is.EqualTo(null));
        
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(userUpdatedUsername.Username), Is.EqualTo(true));
    }
    
    [Test]
    public void UpdateBillUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "test", LastName = "test", Username = "test_12345",
            Password = "test", Email = "test@test.test", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user), Is.EqualTo(false));
        Thread.Sleep(100);
        
        var billDto = new BillDto
        {
            Address = "Tester cel Mare",
            Telephone = "1000000000",
            Country = "Testania",
            City = "Tity",
            PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.BillRepository.UpdateBillToUsername(user.Username, billDto), Is.EqualTo(true));
        Thread.Sleep(100);
        Assert.That(PersistenceAccess.UserRepository.GetBillingDetails(user.Username)?.PostalCode.Equals(billDto.PostalCode), Is.EqualTo(true));
        Thread.Sleep(100);
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username), Is.EqualTo(true));
    }
    
    [Test]
    public void CheckUserGettersUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "test", LastName = "test", Username = "test_12345",
            Password = "test", Email = "test@test.test", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user), Is.EqualTo(true));
        Assert.NotNull(PersistenceAccess.UserRepository.GetUser(user.Username));
        Assert.That(PersistenceAccess.UserRepository.GetAllUsers(), Is.Not.Empty);
        Assert.That(PersistenceAccess.UserRepository.GetBillingDetails(user.Username), Is.Not.Null);
        Assert.That(PersistenceAccess.UserRepository.GetUserPassword(user.Username), Is.Not.Null);
        Assert.That(PersistenceAccess.UserRepository.GetUserType(user.Username), Is.Not.Null);
        
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username), Is.EqualTo(true));
        
        /*TODO use RESULT instead of boolean*/
    }
}