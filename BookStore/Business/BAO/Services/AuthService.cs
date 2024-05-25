using Business.BAO.Interfaces;
using Business.BTO;
using Business.Mappers;
using Business.Utilities;
using Common;
using Persistence.DAL;
using Persistence.DAO;
using Persistence.DTO.User;

namespace Business.BAO.Services;

public class AuthService : IAuth
{
    private const int SessionThresholdMinutes = 5;
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;
    private readonly Dictionary<string, Tuple<string, DateTime>> _sessions = new();

    public Result<string, BaoErrorType> Login(UserLoginBto userLoginBto)
    {
        var gdprUserLoginBto = GdprMapper.DoUserLoginBto(userLoginBto);
        var password = _persistenceFacade.UserRepository.GetUserPassword(gdprUserLoginBto.Username);

        if (!password.IsSuccess)
            return Result<string, BaoErrorType>.Fail(BaoErrorType.UserPasswordNotFound,
                $"No user is registered with {userLoginBto.Username}");

        if (password.SuccessValue != gdprUserLoginBto.Password)
            return Result<string, BaoErrorType>.Fail(BaoErrorType.InvalidPassword,
                $"Invalid password for {userLoginBto.Username}");

        var token = Generator.GetToken();

        _sessions[userLoginBto.Username] = Tuple.Create(token, DateTime.Now.AddMinutes(SessionThresholdMinutes));

        return Result<string, BaoErrorType>.Success(token,
            $"User {userLoginBto.Username} succesfully logged in.");
    }

    public Result<bool, BaoErrorType> Register(UserInfoDto userInfoDto)
    {
        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userInfoDto);

        var result = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);

        if (result.IsSuccess)
            return Result<bool, BaoErrorType>.Success(true,
                $"User {userInfoDto.Username} succesfully logged in.");
        if (result.ErrorType == DaoErrorType.AlreadyRegistered)
            return Result<bool, BaoErrorType>.Fail(BaoErrorType.InvalidRegisterData,
                $"Invalid register data {userInfoDto.Username}");
        return Result<bool, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
            $"Database error while register {userInfoDto.Username}");
    }

    public Result<bool, BaoErrorType> CheckSession(string username, string token)
    {
        if (!_sessions.TryGetValue(username, out var session) || session.Item1 != token)
            return Result<bool, BaoErrorType>.Fail(BaoErrorType.InvalidSession, "Invalid session.");

        if (DateTime.Now <= session.Item2)
            return Result<bool, BaoErrorType>.Success(true, "Session is valid.");

        _sessions.Remove(username);

        return Result<bool, BaoErrorType>.Fail(BaoErrorType.SessionExpired, "Session expired.");
    }

    public Result<bool, BaoErrorType> Logout(string username)
    {
        return !_sessions.Remove(username, out var session)
            ? Result<bool, BaoErrorType>.Fail(BaoErrorType.UserSessionNotFound,
                $"Username {username} has no session registered.")
            : Result<bool, BaoErrorType>.Success(true, $"User {username} succesfully logged out.");
    }
}