using Logger;
using Persistence.DTO;
using Persistence.DTO.Product;

namespace Persistence.DAO.Interfaces;

public interface IProductRepository
{
    Result<bool, DaoErrorType> RegisterProduct(ProductDto productDto);
    Result<ProductDto, DaoErrorType> GetProduct(string name);
    Result<IList<ProductDto>, DaoErrorType> GetAllProducts();
    Result<IList<ProductDto>, DaoErrorType> GetAllProductsByCategory(string category);
    Result<IList<ProductStatsDto>, DaoErrorType> GetAllProductsStats();
    Result<bool, DaoErrorType> UpdatePrice(string name, int newPrice);
    Result<bool, DaoErrorType> UpdateQuantity(string name, int quantity);
    Result<bool, DaoErrorType> DeleteProduct(string name);
}