/**************************************************************************
 *                                                                        *
 *  Description: UserTypeChecker Utility                                  *
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
using Persistence.DAL;

namespace Business.Utilities;

/// <summary>
/// Utility class for checking user types.
/// </summary>
internal static class UserTypeChecker
{
    /// <summary>
    /// Checks if the user is a client.
    /// </summary>
    /// <param name="username">The username of the user to be checked.</param>
    /// <returns>True if the user is a client, otherwise false.</returns>
    public static bool CheckIfClient(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("CLIENT");
    }

    /// <summary>
    /// Checks if the user is a provider.
    /// </summary>
    /// <param name="username">The username of the user to be checked.</param>
    /// <returns>True if the user is a provider, otherwise false.</returns>
    public static bool CheckIfProvider(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("PROVIDER");
    }

    /// <summary>
    /// Checks if the user is an admin.
    /// </summary>
    /// <param name="username">The username of the user to be checked.</param>
    /// <returns>True if the user is an admin, otherwise false.</returns>
    public static bool CheckIfAdmin(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("ADMIN");
    }

    public static LoginMode GetLoginMode(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return LoginMode.None;


        if (userType.SuccessValue == GdprUtility.Encrypt("ADMIN"))
            return LoginMode.Admin;
        return userType.SuccessValue == GdprUtility.Encrypt("PROVIDER") ? LoginMode.Admin : LoginMode.Client;
    }
}