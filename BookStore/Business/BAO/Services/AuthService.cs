/**************************************************************************
 *                                                                        *
 *  Description: Logger Utility                                           *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using Business.BAL;
using Business.BAO.Interfaces;
using Business.BTO;
using Business.Mappers;
using Business.Utilities;
using Common;
using Microsoft.Extensions.Logging;
using Persistence.DAL;

namespace Business.BAO.Services;

internal class AuthService : IAuth
{
    private readonly ILogger _logger = Logger.Instance.GetLogger<AuthService>();

    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;
    private readonly Dictionary<string, Tuple<string, DateTime>> _sessions = new();
    private static readonly Dictionary<string, string> Keys = new();

    private const int SessionThresholdMinutes = 10;

    internal static Result<string, BaoErrorType> GetEncryptionKey(string username)
    {
        var keyToReturn = Keys.FirstOrDefault(s => s.Key == username);

        return keyToReturn.Equals(default(KeyValuePair<string, string>))
            ? Result<string, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, $"No Encryption key for {username}.")
            : Result<string, BaoErrorType>.Success(keyToReturn.Value, $"Encryption key for {username} founded.");
    }

    public Result<string, BaoErrorType> Login(UserLoginBto userLoginBto, LoginMode loginMode)
    {
        if (loginMode != UserTypeChecker.GetLoginMode(userLoginBto.Username))
            return Result<string, BaoErrorType>.Fail(BaoErrorType.InvalidUserType, "Invalid login data.");
        
        var gdprUserLoginBto = GdprMapper.DoUserLoginBto(userLoginBto);
        var password = _persistenceFacade.UserRepository.GetUserPassword(gdprUserLoginBto.Username);

        if (!password.IsSuccess)
            return Result<string, BaoErrorType>.Fail(BaoErrorType.UserPasswordNotFound,
                $"No user is registered with {userLoginBto.Username}");

        _logger.LogInformation(password.Message);

        if (password.SuccessValue != gdprUserLoginBto.Password)
            return Result<string, BaoErrorType>.Fail(BaoErrorType.InvalidPassword,
                $"Invalid password for {userLoginBto.Username}");

        var token = Generator.GetToken();

        _sessions[userLoginBto.Username] = Tuple.Create(token, DateTime.Now.AddMinutes(SessionThresholdMinutes));
        Keys[userLoginBto.Username] = GdprUtility.Hash(userLoginBto.Password);
    
        return Result<string, BaoErrorType>.Success(token,
            $"User {userLoginBto.Username} successfully logged in.");
    }

    public Result<VoidResult, BaoErrorType> CheckSession(string token)
    {
        var sessionToRemove = _sessions.FirstOrDefault(s => s.Value.Item1 == token);
        if (sessionToRemove.Equals(default(KeyValuePair<string, Tuple<string, DateTime>>)))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidSession, "Invalid session empty.");
        
        var keyValuePair =  Keys.FirstOrDefault(s => s.Key == sessionToRemove.Key);
        if (LoginMode.None == UserTypeChecker.GetLoginMode(keyValuePair.Key))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidSession, "User was deleted.");

        if (sessionToRemove.Value.Item1 != token)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidSession, "Invalid session.");

        if (DateTime.Now <= sessionToRemove.Value.Item2)
            return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(), "Session is valid.");

        _sessions.Remove(sessionToRemove.Key);
        Keys.Remove(sessionToRemove.Key);

        return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.SessionExpired, "Session expired.");
    }


    public Result<VoidResult, BaoErrorType> Logout(string? token)
    {
        if (token == null)
            Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserSessionNotFound, $"No session found wiht {token}.");
        
        var sessionToRemove = _sessions.FirstOrDefault(s => s.Value.Item1 == token);

        if (sessionToRemove.Equals(default(KeyValuePair<string, Tuple<string, DateTime>>)))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserSessionNotFound,
                $"User {sessionToRemove.Key} is already logged-out.");
        
        _sessions.Remove(sessionToRemove.Key);
        Keys.Remove(sessionToRemove.Key);

        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
            $"User {sessionToRemove.Key} successfully logged out.");
    }


    public Result<string, BaoErrorType> GetUsername(string? token)
    {
        var session = _sessions.FirstOrDefault(s => s.Value.Item1 == token);

        return session.Equals(default(KeyValuePair<string, Tuple<string, DateTime>>))
            ? Result<string, BaoErrorType>.Fail(BaoErrorType.InvalidSession, "Invalid session.")
            : Result<string, BaoErrorType>.Success(session.Key, "Username retrieved successfully.");
    }
}