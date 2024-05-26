/**************************************************************************
 *                                                                        *
 *  Description: Users Interface                                          *
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

using Common;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Business.BAO.Interfaces;

/// <summary>
/// Defines the methods for user management.
/// </summary>
public interface IUsers
{
    /// <summary>
    /// Registers a new client.
    /// </summary>
    /// <param name="userRegisterDto">The client registration details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> RegisterClient(UserRegisterDto userRegisterDto);

    /// <summary>
    /// Registers a new provider.
    /// </summary>
    /// <param name="requester">The requester of the registration.</param>
    /// <param name="userRegisterDto">The provider registration details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> RegisterProvider(string requester, UserRegisterDto userRegisterDto);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <param name="requester">The requester of the user list. It will be omited.</param>
    /// <returns>A result containing the list of users or an error type.</returns>
    Result<IList<UserInfoDto>, BaoErrorType> GetAllUsers(string requester);

    /// <summary>
    /// Retrieves information for a specific user.
    /// </summary>
    /// <param name="username">The username whose information is to be retrieved.</param>
    /// <returns>A result containing the user information or an error type.</returns>
    Result<UserInfoDto, BaoErrorType> GetUserInfo(string username);

    /// <summary>
    /// Retrieves billing information for a specific user.
    /// </summary>
    /// <param name="username">The username whose billing information is to be retrieved.</param>
    /// <returns>A result containing the billing information or an error type.</returns>
    Result<BillDto, BaoErrorType> GetUserBillInfo(string username);

    /// <summary>
    /// Updates the information of a specific user.
    /// </summary>
    /// <param name="username">The username whose information is to be updated.</param>
    /// <param name="userRegisterDto">The new user registration details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> UpdateUser(string username, UserRegisterDto userRegisterDto);

    /// <summary>
    /// Updates the billing information of a specific user.
    /// </summary>
    /// <param name="username">The username whose billing information is to be updated.</param>
    /// <param name="billDto">The new billing details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> UpdateBill(string username, BillDto billDto);

    /// <summary>
    /// Deletes a specific user.
    /// </summary>
    /// <param name="requester">The requester of the deletion.</param>
    /// <param name="username">The username to be deleted.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> DeleteUser(string requester, string username);
}