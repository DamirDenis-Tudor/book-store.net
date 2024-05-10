using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Interfaces;

public interface IOrderRepository
{
    List<OrderDto> GetOrdersByUsername(string username);

    List<OrderDto> GetOrdersByProductName(string productName);

    List<OrderDto> GetAllOrders();
}