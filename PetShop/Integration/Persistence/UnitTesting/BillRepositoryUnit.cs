using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class BillRepositoryUnit
{
    [SetUp]
    public void PrepareTesting() => PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Testing);
    
    [Test]
    public void UpdateBillUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "testUpdate", LastName = "testUpdate", Username = "test_12345Update",
            Password = "testUpdate", Email = "test@test.testUpdate", UserType = "TESTER"
        };
        Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.Instance.BillRepository?.UpdateBillToUsername(
                user.Username, billDto).IsSuccess,
            Is.EqualTo(true)
        );

        var billingDetails = PersistenceAccess.Instance.BillRepository?.GetBillingDetails(user.Username);
        Assert.That(billingDetails?.IsSuccess, Is.EqualTo(true));
        Assert.That(billingDetails.SuccessValue.PostalCode.Equals(billDto.PostalCode), Is.EqualTo(true));
        Assert.That(PersistenceAccess.Instance.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
    }
}