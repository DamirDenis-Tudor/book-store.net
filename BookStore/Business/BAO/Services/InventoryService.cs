/**************************************************************************
 *                                                                        *
 *  Description: Logger Utility                                           *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using Business.BAO.Interfaces;
using Business.Utilities;
using Common;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DTO.Product;

namespace Business.BAO.Services;

internal class InventoryService : IInventory
{
    private readonly ILogger _logger = Logger.Instance.GetLogger<InventoryService>();
    
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;
    
    public Result<IList<ProductDto>, BaoErrorType> GetInventory()
    {
        var inventory = _persistenceFacade.ProductRepository.GetAllProducts();
       
        _logger.LogInformation(inventory.Message);
        
        return !inventory.IsSuccess
            ? Result<IList<ProductDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductDto>, BaoErrorType>.Success(inventory.SuccessValue);
    }

    // Removed admin access level check
    public Result<IList<ProductStatsDto>, BaoErrorType> GetInventoryStats()
    {
        /*if (!SUserTypeChecker.CheckIfAdmin(username: requester))
            return Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");
*/
        var productStats = _persistenceFacade.ProductRepository.GetAllProductsStats();
        
        _logger.LogInformation(productStats.Message);
        
        return !productStats.IsSuccess
            ? Result<IList<ProductStatsDto>, BaoErrorType>.Fail(BaoErrorType.NoProductRegistered)
            : Result<IList<ProductStatsDto>, BaoErrorType>.Success(productStats.SuccessValue,
                $"Username is not ADMIN.");
    }

    public Result<VoidResult, BaoErrorType> RegisterProduct(string requester, ProductDto productDto)
    {
        var encryptionKey = AuthService.GetEncryptionKey(requester);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);
        
        if (!UserTypeChecker.CheckIfProvider(username: requester, encryptionKey.SuccessValue))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var register = _persistenceFacade.ProductRepository.RegisterProduct(productDto);
        
        _logger.LogInformation(register.Message);
        
        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterProduct, register.Message)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductPrice(string requester, string productName, decimal price)
    {
        var encryptionKey = AuthService.GetEncryptionKey(requester);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);
        
        if (!UserTypeChecker.CheckIfProvider(username: requester, encryptionKey.SuccessValue))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var updatePrice = _persistenceFacade.ProductRepository.UpdatePrice(productName, price);
        
        _logger.LogInformation(updatePrice.Message);
        
        return !updatePrice.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductPrice)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> UpdateProductStocks(string requester, string productName, int quantity)
    {
        var encryptionKey = AuthService.GetEncryptionKey(requester);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);
        
        if (!UserTypeChecker.CheckIfProvider(username: requester, encryptionKey.SuccessValue))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var updateStocks = _persistenceFacade.ProductRepository.UpdateQuantity(productName, quantity);
        
        _logger.LogInformation(updateStocks.Message);
        
        return !updateStocks.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdateProductStocks)
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());
    }

    public Result<VoidResult, BaoErrorType> DeleteProduct(string requester, string productName)
    {
        var encryptionKey = AuthService.GetEncryptionKey(requester);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);
        
        if (!UserTypeChecker.CheckIfProvider(username: requester, encryptionKey.SuccessValue))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not PROVIDER.");

        var delete = _persistenceFacade.ProductRepository.DeleteProduct(productName);
        
        _logger.LogInformation(delete.Message);
        
        if (!delete.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToRegisterProduct,
                $"Username {requester} is not PROVIDER.");
        
        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get());

    }
}