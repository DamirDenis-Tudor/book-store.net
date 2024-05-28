/**************************************************************************
 *                                                                        *
 *  Description: Authentication Interface                                 *
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
using Business.BTO;
using Common;

namespace Business.BAO.Interfaces;

/// <summary>
/// Defines the methods for authentication.
/// </summary>
public interface IAuth
{
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="userLoginBto">The user login details.</param>
    /// <param name="loginMode">The login mode, specific to the presentation layer..</param>
    /// <returns>A result containing a token if successful, or an error type.</returns>
    Result<string, BaoErrorType> Login(UserLoginBto userLoginBto, LoginMode loginMode);

    /// <summary>
    /// Checks if a user session is valid.
    /// </summary>
    /// <param name="token">The session token to check.</param>
    /// <returns>A result indicating whether the session is valid, or an error type.</returns>
    Result<VoidResult, BaoErrorType> CheckSession(string? token);

    /// <summary>
    /// Logs out a user.
    /// </summary>
    /// <param name="token">The session token to log out.</param>
    /// <returns>A result indicating whether the logout was successful, or an error type.</returns>
    Result<VoidResult, BaoErrorType> Logout(string? token);

    /// <summary>
    /// Retrieves the username associated with a given token.
    /// </summary>
    /// <param name="token">The session token to get the username for.</param>
    /// <returns>A result containing the username if successful, or an error type.</returns>
    Result<string, BaoErrorType> GetUsername(string? token);
}

