using Logger;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace UnitTesting;

public class BillRepositoryUnit
{
    [Test]
    public void UpdateBillUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "testUpdate", LastName = "testUpdate", Username = "test_12345Update",
            Password = "testUpdate", Email = "test@test.testUpdate", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.BillRepository.UpdateBillToUsername(
                user.Username, billDto).IsSuccess,
            Is.EqualTo(true)
        );

        var billingDetails = PersistenceAccess.UserRepository.GetBillingDetails(user.Username);
        Assert.That(billingDetails.IsSuccess, Is.EqualTo(true));
        Assert.That(billingDetails.SuccessValue.PostalCode.Equals(billDto.PostalCode), Is.EqualTo(true));
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
    }
}