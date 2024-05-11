using Persistence.DTO;
using Persistence.DTO.Product;

namespace Persistence.DAO.Interfaces;

public interface IProductRepository
{
    bool RegisterProduct(ProductDto productDto);
    ProductDto? GetProduct(string name);
    List<ProductDto?> GetAllProducts();
    List<ProductStatsDto?> GetAllProductsStats();
    bool UpdatePrice(string name, int newPrice);
    bool UpdateQuantity(string name, int quantity);
}