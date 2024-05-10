using Persistence.DTO;

namespace Persistence.DAO.Interfaces;

public interface IOrderRepository
{
    List<OrderDto> GetOrdersByUsername(string username);

    List<OrderDto> GetOrdersByProductName(string productName);

    List<OrderDto> GetAllOrders();
}