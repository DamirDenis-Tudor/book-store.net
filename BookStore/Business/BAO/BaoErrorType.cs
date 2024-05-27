/**************************************************************************
 *                                                                        *
 *  Description: BaoErrorType                                             *
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

namespace Business.BAO;

/// <summary>
/// Enumeration representing various types of errors that can occur in the DAO (Data Access Object) layer.
/// </summary>
public enum BaoErrorType
{
    /// <summary>
    /// Indicates that the requested user password was not found.
    /// </summary>
    UserPasswordNotFound,

    /// <summary>
    /// Indicates that the user session was not found.
    /// </summary>
    UserSessionNotFound,

    /// <summary>
    /// Indicates that no users were found.
    /// </summary>
    UsersNotFound,

    /// <summary>
    /// Indicates that the user is not allowed to perform the action.
    /// </summary>
    UserNotAllowed,

    /// <summary>
    /// Indicates that the provided password is invalid.
    /// </summary>
    InvalidPassword,

    /// <summary>
    /// Indicates that the provided registration data is invalid.
    /// </summary>
    InvalidRegisterData,

    /// <summary>
    /// Indicates a general database error.
    /// </summary>
    DatabaseError,

    /// <summary>
    /// Indicates that the session has expired.
    /// </summary>
    SessionExpired,

    /// <summary>
    /// Indicates that the session is invalid.
    /// </summary>
    InvalidSession,

    /// <summary>
    /// Indicates that the requested product was not found.
    /// </summary>
    ProductNotFound,

    /// <summary>
    /// Indicates insufficient stock for the product.
    /// </summary>
    InsufficienciesStock,

    /// <summary>
    /// Indicates that the user has no orders.
    /// </summary>
    UserHasNoOrders,

    /// <summary>
    /// Indicates that no products are registered.
    /// </summary>
    NoProductRegistered,

    /// <summary>
    /// Indicates a failure to register a product.
    /// </summary>
    FailedToRegisterProduct,

    /// <summary>
    /// Indicates a failure to update the product price.
    /// </summary>
    FailedToUpdateProductPrice,

    /// <summary>
    /// Indicates a failure to update the product stocks.
    /// </summary>
    FailedToUpdateProductStocks,

    /// <summary>
    /// Indicates a failure to delete a product.
    /// </summary>
    FailedToDeleteAProduct,

    /// <summary>
    /// Indicates that the user type is invalid.
    /// </summary>
    InvalidUserType,

    /// <summary>
    /// Indicates a failure to register an order.
    /// </summary>
    FailedToRegisterOrder
}