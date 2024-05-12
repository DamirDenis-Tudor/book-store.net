using Persistence.DAL;
using Persistence.DTO.User;

namespace UnitTesting;

public class UserRepositoryUnit
{
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
                Assert.That(PersistenceAccess.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceAccess.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(false));

                Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(false));
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
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(true));

        var userUpdated = new UserInfoDto
        {
            FirstName = "NonTest", LastName = "", Username = "nonTest",
            Password = "", Email = "", UserType = ""
        };

        Assert.That(PersistenceAccess.UserRepository.UpdateUser(user.Username, userUpdated).IsSuccess,
            Is.EqualTo(true));

        var fetchedUser = PersistenceAccess.UserRepository.GetUser(userUpdated.Username);
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
                    PersistenceAccess.UserRepository.UpdateUser(userUpdated.Username, userUpdatedUsername).IsSuccess,
                    Is.EqualTo(true)
                );

                Assert.That(PersistenceAccess.UserRepository.GetUser(user.Username).IsSuccess, Is.EqualTo(false));
            }
        );
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(
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
            Assert.That(PersistenceAccess.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.UserRepository.GetUser(user.Username).IsSuccess, Is.EqualTo(true));
            
            var allUsersResult = PersistenceAccess.UserRepository.GetAllUsers();
            Assert.That(allUsersResult.IsSuccess, Is.EqualTo(true));
            Assert.IsNotEmpty(allUsersResult.SuccessValue);

            Assert.That(PersistenceAccess.UserRepository.GetUserPassword(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.UserRepository.GetUserType(user.Username).IsSuccess, Is.EqualTo(true));

            Assert.That(PersistenceAccess.UserRepository.DeleteUser(user.Username).IsSuccess, Is.EqualTo(true));
        });
    }
}