using Microsoft.EntityFrameworkCore;
using Persistence.DAO.Interfaces;
using Persistence.DTO;

namespace Persistence.DAO.Repositories;

internal class ProductRepository(DbContext dbContext) : IProductRepository
{
    private DbContext _dbContext = dbContext;
    
    public ProductDto? GetProduct(string name)
    {
        throw new NotImplementedException();
    }

    public List<ProductDto>? GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public bool UpdatePrice(string name, int newPrice)
    {
        throw new NotImplementedException();
    }

    public bool UpdateQuantity(string name, int quantity)
    {
        throw new NotImplementedException();
    }
}