using Persistence.DAL;
using Persistence.DTO.User;

namespace UnitTesting.PersistenceTesting;

public class UserRepositoryUnit
{
    [SetUp]
    public void PrepareTesting() => PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Testing);
    [Test]
    public void CreateAndDeleteUserUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
            Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
        };
        Assert.Multiple(() =>
            {
                Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(false));

                Assert.That(PersistenceAccess.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceAccess.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(false));
            }
        );
    }

    [Test]
    public void UpdateUserUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "testUpdate", LastName = "testUpdate", Username = "test_12345Update",
            Password = "testUpdate", Email = "test@test.testUpdate", UserType = "TESTERUpdate"
        };
        Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var userUpdated = new UserInfoDto
        {
            FirstName = "NonTest", LastName = "", Username = "nonTest",
            Password = "", Email = "", UserType = ""
        };

        Assert.That(PersistenceAccess.Instance.UserRepository.UpdateUser(user.Username, userUpdated).IsSuccess,
            Is.EqualTo(true));

        var fetchedUser = PersistenceAccess.Instance.UserRepository.GetUser(userUpdated.Username);
        Assert.Multiple(() =>
            {
                Assert.That(fetchedUser.IsSuccess, Is.EqualTo(true));
                Assert.That(fetchedUser.SuccessValue.FirstName, Is.EqualTo("NonTest"));
            }
        );


        var userUpdatedUsername = new UserInfoDto
        {
            FirstName = "test1", LastName = "", Username = "test",
            Password = "", Email = "", UserType = ""
        };
        Assert.Multiple(() =>
            {
                Assert.That(
                    PersistenceAccess.Instance.UserRepository.UpdateUser(userUpdated.Username, userUpdatedUsername).IsSuccess,
                    Is.EqualTo(true)
                );

                Assert.That(PersistenceAccess.Instance.UserRepository.GetUser(user.Username).IsSuccess, Is.EqualTo(false));
            }
        );
        Assert.That(PersistenceAccess.Instance.UserRepository.DeleteUser(
                userUpdatedUsername.Username).IsSuccess,
            Is.EqualTo(true)
        );
    }

    [Test]
    public void CheckUserGettersUnitTest()
    {
        var user = new UserInfoDto
        {
            FirstName = "testGetters", LastName = "testGetters", Username = "test_12345Getters",
            Password = "testGetters", Email = "test@test.testGetters", UserType = "TESTERGetters"
        };
        Assert.Multiple(() =>
        {
            Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.Instance.UserRepository?.GetUser(user.Username).IsSuccess, Is.EqualTo(true));
            
            var allUsersResult = PersistenceAccess.Instance.UserRepository?.GetAllUsers();
            Assert.That(allUsersResult?.IsSuccess, Is.EqualTo(true));
            Assert.IsNotEmpty(allUsersResult?.SuccessValue!);

            Assert.That(PersistenceAccess.Instance.UserRepository?.GetUserPassword(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.Instance.UserRepository?.GetUserType(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.Instance.UserRepository?.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
        });
    }
}