/**************************************************************************
 *                                                                        *
 *  Description: IOrderRepository                                         *
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
using Persistence.DTO.Order;

namespace Persistence.DAO.Interfaces;

/// <summary>
/// Interface for Order Repository operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Registers a new order session.
    /// </summary>
    /// <param name="orderSessionDto">The order session data transfer object containing the details of the order session to register.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> RegisterOrderSession(OrderSessionDto orderSessionDto);

    /// <summary>
    /// Deletes an order session by its session code.
    /// </summary>
    /// <param name="sessionCode">The session code of the order session to delete.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> DeleteOrderSession(string sessionCode);

    /// <summary>
    /// Retrieves an order session by its session code.
    /// </summary>
    /// <param name="sessionCode">The session code of the order session to retrieve.</param>
    /// <returns>A Result containing either the OrderSessionDto or a DaoErrorType indicating the type of error.</returns>
    Result<OrderSessionDto, DaoErrorType> GetSessionOrder(string sessionCode);

    /// <summary>
    /// Retrieves all order sessions for a specific user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve order sessions for.</param>
    /// <returns>A Result containing either a list of OrderSessionDto or a DaoErrorType indicating the type of error.</returns>
    Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrdersByUsername(string username);

    /// <summary>
    /// Retrieves all order sessions.
    /// </summary>
    /// <returns>A Result containing either a list of OrderSessionDto or a DaoErrorType indicating the type of error.</returns>
    Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrders();
    
}