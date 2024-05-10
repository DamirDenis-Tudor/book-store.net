using Microsoft.EntityFrameworkCore;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Repositories;

internal class OrderRepository(DbContext dbContext) : IOrderRepository
{
    private DbContext _dbContext = dbContext;

    public List<OrderDto> GetOrdersByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public List<OrderDto> GetOrdersByProductName(string productName)
    {
        throw new NotImplementedException();
    }

    public List<OrderDto> GetAllOrders()
    {
        throw new NotImplementedException();
    }
}