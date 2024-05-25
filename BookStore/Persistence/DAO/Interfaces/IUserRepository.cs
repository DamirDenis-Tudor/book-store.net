/**************************************************************************
 *                                                                        *
 *  Description: IUserRepository                                          *
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
using Persistence.DTO.User;

namespace Persistence.DAO.Interfaces;

/// <summary>
/// Interface for User Repository operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userInfo">The user data transfer object containing the details of the user to register.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> RegisterUser(UserInfoDto userInfo);

    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="username">The username of the user to update.</param>
    /// <param name="userDtoInfoDto">The user data transfer object containing the new details of the user.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> UpdateUser(string username, UserInfoDto userDtoInfoDto);

    /// <summary>
    /// Deletes a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to delete.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> DeleteUser(string username);

    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>A Result containing either a list of BillUserDto or a DaoErrorType indicating the type of error.</returns>
    Result<List<BillUserDto>, DaoErrorType> GetAllUsers();

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A Result containing either the BillUserDto or a DaoErrorType indicating the type of error.</returns>
    Result<BillUserDto, DaoErrorType> GetUser(string username);

    /// <summary>
    /// Retrieves the password of a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve the password for.</param>
    /// <returns>A Result containing either the user's password or a DaoErrorType indicating the type of error.</returns>
    Result<string, DaoErrorType> GetUserPassword(string username);

    /// <summary>
    /// Retrieves the type of a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve the type for.</param>
    /// <returns>A Result containing either the user's type or a DaoErrorType indicating the type of error.</returns>
    Result<string, DaoErrorType> GetUserType(string username);
}