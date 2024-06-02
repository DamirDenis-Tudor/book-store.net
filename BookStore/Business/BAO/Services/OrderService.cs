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

using Business.BAO.Interfaces;
using Business.BTO;
using Common;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace Business.BAO.Services;

internal class OrderService : IOrder
{
    private readonly ILogger _logger = Logger.Instance.GetLogger<OrderService>();

    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;

    public Result<VoidResult, BaoErrorType> PlaceOrder(OrderBto orderBto)
    {
        var orderSessionDto = new OrderSessionDto
        {
            Username = orderBto.Username,
            SessionCode = new Random().Next(100000).ToString(),
            Status = "Pending",
            TotalPrice = 0m,
            OrderProducts = []
        };

        var updateList = new Dictionary<string, int>();
        foreach (var orderItem in orderBto.OrderItemBtos)
        {
            var product = _persistenceFacade.ProductRepository.GetProduct(orderItem.ProductName);

            _logger.LogInformation(product.Message);

            if (!product.IsSuccess)
                return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.ProductNotFound,
                    $"No product with name {orderItem.ProductName} found");

            if (product.SuccessValue.Quantity < orderItem.OrderQuantity)
                return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InsufficienciesStock,
                    $"Product {orderItem.ProductName} has insufficient quantity.");

            updateList[orderItem.ProductName] = product.SuccessValue.Quantity - orderItem.OrderQuantity;

            var price = product.SuccessValue.Price * orderItem.OrderQuantity;
            orderSessionDto.OrderProducts.Add(new OrderProductDto
            {
                SessionCode = orderSessionDto.SessionCode,
                OrderQuantity = orderItem.OrderQuantity,
                Price = price,
                ProductInfoDto = new ProductInfoDto
                {
                    Name = orderItem.ProductName,
                    Description = product.SuccessValue.ProductInfoDto.Description,
                    Link = product.SuccessValue.ProductInfoDto.Link,
                    Category = product.SuccessValue.ProductInfoDto.Category,
                }
            });

            orderSessionDto.TotalPrice += price;
        }

        var registerOrder = _persistenceFacade.OrderRepository.RegisterOrderSession(orderSessionDto);

        _logger.LogInformation(registerOrder.Message);

        if (!registerOrder.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterOrder, registerOrder.Message);

        updateList.ToList().ForEach(pair =>
        {
            var update = _persistenceFacade.ProductRepository.UpdateQuantity(pair.Key, pair.Value);

            _logger.LogInformation(update.Message);
        });

        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
            $"Order {orderSessionDto.SessionCode} placed successfully.");
    }

    public Result<IList<OrderSessionDto>, BaoErrorType> GetUserOrders(string username)
    {
        var orders = _persistenceFacade.OrderRepository.GetAllOrdersByUsername(username);

        _logger.LogInformation(orders.Message);

        return !orders.IsSuccess
            ? Result<IList<OrderSessionDto>, BaoErrorType>.Fail(BaoErrorType.UserHasNoOrders)
            : Result<IList<OrderSessionDto>, BaoErrorType>.Success(orders.SuccessValue);
    }
}