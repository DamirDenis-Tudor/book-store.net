using Business.BTO;
using Common;
using Persistence.DTO.User;

namespace Business.BAO.Interfaces;

public interface IAuthentication
{
    Result<string, BaoErrorType> Login(UserLoginBto userLoginBto);
    Result<bool, BaoErrorType> Register(UserInfoDto userInfoDto);
    Result<bool, BaoErrorType> CheckSession(string username, string token);
    Result<bool, BaoErrorType> Logout(string username);
}