using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Interfaces;

public interface IProductRepository
{
    ProductDto? GetProduct(string name);
    List<ProductDto>? GetAllProducts();
    
    bool UpdatePrice(string name, int newPrice);
    bool UpdateQuantity(string name, int quantity);
}