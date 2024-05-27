/**************************************************************************
 *                                                                        *
 *  Description: GDPR Mapper Utility                                      *
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

using Business.BTO;
using Business.Utilities;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.User;

namespace Business.Mappers;

/// <summary>
/// Utility class for mapping user and bill data according to GDPR (General Data Protection Regulation) standards.
/// </summary>
internal static class GdprMapper
{
    /// <summary>
    /// Maps the user registration data to GDPR compliant format.
    /// </summary>
    /// <param name="userRegisterDto">The user registration data to be mapped.</param>
    /// <returns>The user registration data mapped to GDPR compliant format.</returns>
    public static UserRegisterDto DoUserInfoDtoGdpr(UserRegisterDto userRegisterDto)
    {
        return new UserRegisterDto
        {
            FirstName = GdprUtility.Encrypt(userRegisterDto.FirstName),
            LastName = GdprUtility.Encrypt(userRegisterDto.LastName),
            Username = GdprUtility.Encrypt(userRegisterDto.Username),
            Password = GdprUtility.Hash(userRegisterDto.Password),
            Email = GdprUtility.Encrypt(userRegisterDto.Email),
            UserType = GdprUtility.Encrypt(userRegisterDto.UserType)
        };
    }

    /// <summary>
    /// Reverts the GDPR compliant mapping of user information data for billing purposes.
    /// </summary>
    /// <param name="encryptedUserInfoDto">The GDPR compliant user information data to be reverted.</param>
    /// <returns>The reverted user information data for billing purposes.</returns>
    public static UserInfoDto UndoUserInfoDtoGdpr(UserInfoDto encryptedUserInfoDto)
    {
        return new UserInfoDto
        {
            FirstName = GdprUtility.Decrypt(encryptedUserInfoDto.FirstName),
            LastName = GdprUtility.Decrypt(encryptedUserInfoDto.LastName),
            Username = GdprUtility.Decrypt(encryptedUserInfoDto.Username),
            Email = GdprUtility.Decrypt(encryptedUserInfoDto.Email),
        };
    }

    /// <summary>
    /// Maps the billing data to GDPR compliant format.
    /// </summary>
    /// <param name="billDto">The billing data to be mapped.</param>
    /// <returns>The billing data mapped to GDPR compliant format.</returns>
    public static BillDto DoBillGdpr(BillDto billDto)
    {
        return new BillDto
        {
            Address = GdprUtility.Encrypt(billDto.Address),
            Telephone = GdprUtility.Encrypt(billDto.Telephone),
            Country = GdprUtility.Encrypt(billDto.Country),
            City = GdprUtility.Encrypt(billDto.City),
            PostalCode = GdprUtility.Encrypt(billDto.PostalCode)
        };
    }

    /// <summary>
    /// Reverts the GDPR compliant mapping of billing data.
    /// </summary>
    /// <param name="encryptedBillDto">The GDPR compliant billing data to be reverted.</param>
    /// <returns>The reverted billing data.</returns>
    public static BillDto UndoBillGdpr(BillDto encryptedBillDto)
    {
        return new BillDto
        {
            Address = GdprUtility.Decrypt(encryptedBillDto.Address),
            Telephone = GdprUtility.Decrypt(encryptedBillDto.Telephone),
            Country = GdprUtility.Decrypt(encryptedBillDto.Country),
            City = GdprUtility.Decrypt(encryptedBillDto.City),
            PostalCode = GdprUtility.Decrypt(encryptedBillDto.PostalCode)
        };
    }

    /// <summary>
    /// Reverts the GDPR compliant mapping of order session data.
    /// </summary>
    /// <param name="encryptedOrderSessionDto">The GDPR compliant order session data to be reverted.</param>
    /// <returns>The reverted order session data.</returns>
    public static OrderSessionDto UndoOrderSessionDtoGdpr(OrderSessionDto encryptedOrderSessionDto)
    {
        encryptedOrderSessionDto.Username = GdprUtility.Decrypt(encryptedOrderSessionDto.Username);
        return encryptedOrderSessionDto;
    }

    /// <summary>
    /// Maps the order data to GDPR compliant format.
    /// </summary>
    /// <param name="orderBto">The order data to be mapped.</param>
    /// <returns>The order data mapped to GDPR compliant format.</returns>
    public static OrderBto DoOrderBto(OrderBto orderBto)
    {
        orderBto.Username = GdprUtility.Encrypt(orderBto.Username);
        return orderBto;
    }
        
    /// <summary>
    /// Maps the user login data to GDPR compliant format.
    /// </summary>
    /// <param name="userLoginBto">The user login data to be mapped.</param>
    /// <returns>The user login data mapped to GDPR compliant format.</returns>
    public static UserLoginBto DoUserLoginBto(UserLoginBto userLoginBto)
    {
        return new UserLoginBto
        {
            Username = GdprUtility.Encrypt(userLoginBto.Username),
            Password = GdprUtility.Hash(userLoginBto.Password),
        };
    }
}