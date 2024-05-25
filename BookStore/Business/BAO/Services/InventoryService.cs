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

    private bool CheckIfAdmin(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = _persistenceFacade.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("ADMIN");
    }

    public Result<IList<ProductDto>, BaoErrorType> GetInventory()
    {
        var inventory = _persistenceFacade.ProductRepository.GetAllProducts();

        return !inventory.IsSuccess
            ? Result<IList<ProductDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductDto>, BaoErrorType>.Success(inventory.SuccessValue);
    }

    public Result<IList<ProductStatsDto>, BaoErrorType> GetInventoryStats(string username)
    {
        if (!CheckIfAdmin(username: username))
            return Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {username} is not ADMIN.");

        var productStats = _persistenceFacade.ProductRepository.GetAllProductsStats();
        return !productStats.IsSuccess
            ? Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductStatsDto>, BaoErrorType>.Success(productStats.SuccessValue,
                $"Username {username} is not ADMIN.");
    }

    public Result<VoidResult, BaoErrorType> RegisterProduct(string username, ProductDto productDto)
    {
        if (!CheckIfAdmin(username: username))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {username} is not ADMIN.");

        var register = _persistenceFacade.ProductRepository.RegisterProduct(productDto);
        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterProduct)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductPrice(string username, string productName, decimal price)
    {
        if (!CheckIfAdmin(username: username))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {username} is not ADMIN.");

        var updatePrice = _persistenceFacade.ProductRepository.UpdatePrice(productName, price);
        return !updatePrice.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductPrice)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductStocks(string username, string productName, int quantity)
    {
        if (!CheckIfAdmin(username: username))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {username} is not ADMIN.");

        var updateStocks = _persistenceFacade.ProductRepository.UpdateQuantity(productName, quantity);
        return !updateStocks.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductStocks)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }
}