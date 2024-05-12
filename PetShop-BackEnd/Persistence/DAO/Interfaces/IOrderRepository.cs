using Logger;
using Persistence.DTO;
using Persistence.DTO.Order;

namespace Persistence.DAO.Interfaces;

public interface IOrderRepository
{
    Result<bool, DaoErrorType> RegisterOrderSession(OrderSessionDto orderSessionDto);
    Result<bool, DaoErrorType> DeleteOrderSession(string sessionCode);
    Result<OrderSessionDto, DaoErrorType> GetSessionOrder(string sessionCode);
    Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrdersByUsername(string username);
    
    Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrders();
}