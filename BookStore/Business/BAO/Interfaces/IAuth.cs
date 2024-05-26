using Business.BTO;
using Common;
using Persistence.DTO.User;

namespace Business.BAO.Interfaces;

public interface IAuth
{
    Result<string, BaoErrorType> Login(UserLoginBto userLoginBto);

    Result<bool, BaoErrorType> CheckSession(string username, string token);
    Result<bool, BaoErrorType> Logout(string username);
}