using Persistence.DAL;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class UserRepositoryUnit
{
    [SetUp]
    public void PrepareTesting() => PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
    [Test]
    public void CreateAndDeleteUserUnitTest()
    {
        var user = new UserRegisterDto
        {
            FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
            Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
        };
        Assert.Multiple(() =>
            {
                Assert.That(PersistenceFacade.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceFacade.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(false));

                Assert.That(PersistenceFacade.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceFacade.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(false));
            }
        );
    }

    [Test]
    public void UpdateUserUnitTest()
    {
        var user = new UserRegisterDto
        {
            FirstName = "testUpdate", LastName = "testUpdate", Username = "test_12345Update",
            Password = "testUpdate", Email = "test@test.testUpdate", UserType = "TESTERUpdate"
        };
        Assert.That(PersistenceFacade.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var userUpdated = new UserRegisterDto
        {
            FirstName = "NonTest", LastName = "", Username = "nonTest",
            Password = "", Email = "", UserType = ""
        };

        Assert.That(PersistenceFacade.Instance.UserRepository.UpdateUser(user.Username, userUpdated).IsSuccess,
            Is.EqualTo(true));

        var fetchedUser = PersistenceFacade.Instance.UserRepository.GetUser(userUpdated.Username);
        Assert.Multiple(() =>
            {
                Assert.That(fetchedUser.IsSuccess, Is.EqualTo(true));
                Assert.That(fetchedUser.SuccessValue.FirstName, Is.EqualTo("NonTest"));
            }
        );


        var userUpdatedUsername = new UserRegisterDto
        {
            FirstName = "test1", LastName = "", Username = "test",
            Password = "", Email = "", UserType = ""
        };
        Assert.Multiple(() =>
            {
                Assert.That(
                    PersistenceFacade.Instance.UserRepository.UpdateUser(userUpdated.Username, userUpdatedUsername).IsSuccess,
                    Is.EqualTo(true)
                );

                Assert.That(PersistenceFacade.Instance.UserRepository.GetUser(user.Username).IsSuccess, Is.EqualTo(false));
            }
        );
        Assert.That(PersistenceFacade.Instance.UserRepository.DeleteUser(
                userUpdatedUsername.Username).IsSuccess,
            Is.EqualTo(true)
        );
    }

    [Test]
    public void CheckUserGettersUnitTest()
    {
        var user = new UserRegisterDto
        {
            FirstName = "testGetters", LastName = "testGetters", Username = "test_12345Getters",
            Password = "testGetters", Email = "test@test.testGetters", UserType = "TESTERGetters"
        };
        Assert.Multiple(() =>
        {
            Assert.That(PersistenceFacade.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceFacade.Instance.UserRepository?.GetUser(user.Username).IsSuccess, Is.EqualTo(true));
            
            var allUsersResult = PersistenceFacade.Instance.UserRepository?.GetAllUsers();
            Assert.That(allUsersResult?.IsSuccess, Is.EqualTo(true));
            Assert.IsNotEmpty(allUsersResult?.SuccessValue!);

            Assert.That(PersistenceFacade.Instance.UserRepository?.GetUserPassword(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceFacade.Instance.UserRepository?.GetUserType(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceFacade.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
        });
    }
}