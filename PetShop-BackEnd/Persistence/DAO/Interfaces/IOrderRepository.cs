using Persistence.DTO;

namespace Persistence.DAO.Interfaces;

public interface IOrderRepository
{
    List<OrderSessionDto> GetOrdersByUsername(string username);

    List<OrderSessionDto> GetOrdersByProductName(string productName);

    List<OrderSessionDto> GetAllOrders();
}