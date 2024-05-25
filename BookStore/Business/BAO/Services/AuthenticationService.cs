using Business.BAO.Interfaces;
using Business.BTO;
using Common;
using Persistence.DTO.User;

namespace Business.BAO.Services;

public class AuthenticationService: IAuthentication
{
    public Result<string, BaoErrorType> Login(UserLoginBto userLoginBto)
    {
        var gdprUserLoginBto = new UserLoginBto
        {
            Username = GdprUtility.Encrypt(userLoginBto.Username),
            Password = GdprUtility.Hash(userLoginBto.Password)
        };
        
        throw new NotImplementedException();
    }

    public Result<bool, BaoErrorType> Register(UserInfoDto userInfoDto)
    {
        throw new NotImplementedException();
    }

    public Result<bool, BaoErrorType> CheckSession(string token)
    {
        throw new NotImplementedException();
    }
}