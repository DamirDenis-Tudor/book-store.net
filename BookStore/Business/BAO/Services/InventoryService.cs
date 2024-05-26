using Business.BAO.Interfaces;
using Business.Mappers;
using Business.Utilities;
using Common;
using Persistence.DAL;
using Persistence.DTO.Product;

namespace Business.BAO.Services;

public class InventoryService : IInventory
{
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;
    
    public Result<IList<ProductDto>, BaoErrorType> GetInventory()
    {
        var inventory = _persistenceFacade.ProductRepository.GetAllProducts();

        return !inventory.IsSuccess
            ? Result<IList<ProductDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductDto>, BaoErrorType>.Success(inventory.SuccessValue);
    }

    public Result<IList<ProductStatsDto>, BaoErrorType> GetInventoryStats(string requester)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        var productStats = _persistenceFacade.ProductRepository.GetAllProductsStats();
        return !productStats.IsSuccess
            ? Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductStatsDto>, BaoErrorType>.Success(productStats.SuccessValue,
                $"Username {requester} is not ADMIN.");
    }

    public Result<VoidResult, BaoErrorType> RegisterProduct(string requester, ProductDto productDto)
    {
        if (!UserTypeChecker.CheckIfProvider(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var register = _persistenceFacade.ProductRepository.RegisterProduct(productDto);
        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterProduct)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductPrice(string requester, string productName, decimal price)
    {
        if (!UserTypeChecker.CheckIfProvider(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var updatePrice = _persistenceFacade.ProductRepository.UpdatePrice(productName, price);
        return !updatePrice.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductPrice)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductStocks(string requester, string productName, int quantity)
    {
        if (!UserTypeChecker.CheckIfProvider(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var updateStocks = _persistenceFacade.ProductRepository.UpdateQuantity(productName, quantity);
        return !updateStocks.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductStocks)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> DeleteProduct(string requester, string productName)
    {
        if (!UserTypeChecker.CheckIfProvider(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var delete = _persistenceFacade.ProductRepository.DeleteProduct(productName);
        if (!delete.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterProduct,
                $"Username {requester} is not PROVIDER.");
        
        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());

    }
}