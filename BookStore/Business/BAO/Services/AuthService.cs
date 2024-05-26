using Business.BAO.Interfaces;
using Business.BTO;
using Business.Mappers;
using Business.Utilities;
using Common;
using Persistence.DAL;

namespace Business.BAO.Services;

internal class AuthService : IAuth
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