using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;

namespace Persistence.DAO.Repositories;

internal class OrderRepository(PersistenceAccess.DatabaseContext dbContext) : IOrderRepository
{

    public List<OrderSessionDto> GetOrdersByUsername(string username)
    {
        /*var orders = dbContext.Orders
            .Include(o => o.User)
            .Where(o => o.User.Username == username)
            .Select(o => MapperDto.)
                    
    
        return orders;*/
        throw new NotImplementedException();
    }

    public List<OrderSessionDto> GetOrdersByProductName(string productName)
    {
        throw new NotImplementedException();
    }

    public List<OrderSessionDto> GetAllOrders()
    {
        throw new NotImplementedException();
    }
    
    
}