using Business.BTO;
using Common;
using Persistence.DTO.Order;

namespace Business.BAO.Interfaces;

public interface IOrder
{
    Result<VoidResult, BaoErrorType> PlaceOrder(OrderBto orderBto);
    Result<IList<OrderSessionDto>, BaoErrorType> GetUserOrders(string username);
}