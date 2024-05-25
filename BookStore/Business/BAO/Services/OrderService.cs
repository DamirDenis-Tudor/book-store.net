using Business.BAO.Interfaces;
using Business.BTO;
using Business.Mappers;
using Business.Utilities;
using Common;
using Persistence.DAL;
using Persistence.DTO.Order;

namespace Business.BAO.Services;

public class OrderService : IOrder
{
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;

    public Result<VoidResult, BaoErrorType> PlaceOrder(OrderBto orderBto)
    {
        orderBto = GdprMapper.DoOrderBto(orderBto);

        var orderSessionDto = new OrderSessionDto
        {
            Username = orderBto.Username,
            SessionCode = Generator.GetToken(),
            Status = "Pending",
            TotalPrice = 0m,
            OrderProducts = []
        };

        foreach (var orderItem in orderBto.OrderItemBtos)
        {
            var product = _persistenceFacade.ProductRepository.GetProduct(orderItem.ProductName);

            if (!product.IsSuccess)
                return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.ProductNotFound,
                    $"No product with name {orderItem.ProductName} found");

            if (product.SuccessValue.Quantity < orderItem.OrderQuantity)
                return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InsufficienciesStock,
                    $"Product {orderItem.ProductName} has insufficient quantity.");

            _persistenceFacade.ProductRepository.UpdateQuantity(orderItem.ProductName,
                product.SuccessValue.Quantity - orderItem.OrderQuantity);
            var price = product.SuccessValue.Price * orderItem.OrderQuantity;
            orderSessionDto.OrderProducts.Add(new OrderProductDto
            {
                ProductName = orderItem.ProductName,
                Description = product.SuccessValue.Description,
                SessionCode = orderSessionDto.SessionCode,
                OrderQuantity = orderItem.OrderQuantity,
                Link = product.SuccessValue.Link,
                Price = price
            });

            orderSessionDto.TotalPrice += price;
        }

        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
            $"Order {orderSessionDto.SessionCode} placed succefully.");
    }

    public Result<IList<OrderSessionDto>, BaoErrorType> GetUserOrders(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        
        var orders = _persistenceFacade.OrderRepository.GetAllOrdersByUsername(gdprUsername);
        
        if (!orders.IsSuccess)
            return Result<IList<OrderSessionDto>, BaoErrorType>.Fail(BaoErrorType.UserHasNoOrders);
        
        for (var i = 0; i < orders.SuccessValue.Count; i++)
        {
            orders.SuccessValue[i] = GdprMapper.UndoOrderSessionDtoGdpr(orders.SuccessValue[i]);
        }
        
        return Result<IList<OrderSessionDto>, BaoErrorType>.Success(orders.SuccessValue);
    }
}