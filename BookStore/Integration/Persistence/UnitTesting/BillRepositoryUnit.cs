using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class BillRepositoryUnit
{
    [SetUp]
    public void PrepareTesting() => PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
    
    [Test]
    public void UpdateBillUnitTest()
    {
        var user = new UserRegisterDto
        {
            FirstName = "testUpdate", LastName = "testUpdate", Username = "test_12345Update",
            Password = "testUpdate", Email = "test@test.testUpdate", UserType = "TESTER"
        };
        Assert.That(PersistenceFacade.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceFacade.Instance.BillRepository?.UpdateBillByUsername(
                user.Username, billDto).IsSuccess,
            Is.EqualTo(true)
        );

        var billingDetails = PersistenceFacade.Instance.BillRepository?.GetBillingDetails(user.Username);
        Assert.That(billingDetails?.IsSuccess, Is.EqualTo(true));
        Assert.That(billingDetails.SuccessValue.PostalCode.Equals(billDto.PostalCode), Is.EqualTo(true));
        Assert.That(PersistenceFacade.Instance.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
    }
}