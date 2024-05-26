/**************************************************************************
 *                                                                        *
 *  Description: Order Interface                                          *
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
using Common;
using Persistence.DTO.Order;

namespace Business.BAO.Interfaces;

/// <summary>
/// Defines the methods for order management.
/// </summary>
public interface IOrder
{
    /// <summary>
    /// Places a new order.
    /// </summary>
    /// <param name="orderBto">The order details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> PlaceOrder(OrderBto orderBto);

    /// <summary>
    /// Retrieves orders for a specific user.
    /// </summary>
    /// <param name="username">The username whose orders are to be retrieved.</param>
    /// <returns>A result containing a list of user orders or an error type.</returns>
    Result<IList<OrderSessionDto>, BaoErrorType> GetUserOrders(string username);
}