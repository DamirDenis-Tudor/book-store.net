using Common;
using Persistence.DTO.Product;

namespace Business.BAO.Interfaces;

public interface IInventory
{
    Result<IList<ProductDto>, BaoErrorType> GetInventory();
    Result<IList<ProductStatsDto>, BaoErrorType> GetInventoryStats(string requester);
    Result<VoidResult, BaoErrorType> RegisterProduct(string requester, ProductDto productDto);
    Result<VoidResult, BaoErrorType> UpdateProductPrice(string username, string productName, decimal price);
    Result<VoidResult, BaoErrorType> UpdateProductStocks(string username, string productName, int quantity);
    Result<VoidResult, BaoErrorType> DeleteProduct(string requester, string productName);
}