/**************************************************************************
 *                                                                        *
 *  Description: Inventory Interface                                      *
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

using Common;
using Persistence.DTO.Product;

namespace Business.BAO.Interfaces;

/// <summary>
/// Defines the methods for inventory management.
/// </summary>
public interface IInventory
{
    /// <summary>
    /// Retrieves the inventory list.
    /// </summary>
    /// <returns>A result containing the list of products or an error type.</returns>
    Result<IList<ProductDto>, BaoErrorType> GetInventory();

    /// <summary>
    /// Retrieves inventory statistics.
    /// </summary>
    /// <returns>A result containing the inventory statistics or an error type.</returns>
    Result<IList<ProductStatsDto>, BaoErrorType> GetInventoryStats();

    /// <summary>
    /// Registers a new product.
    /// </summary>
    /// <param name="requester">The requester of the product registration.</param>
    /// <param name="productDto">The product details.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> RegisterProduct(string requester, ProductDto productDto);

    /// <summary>
    /// Updates the price of a product.
    /// </summary>
    /// <param name="username">The username requesting the update.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="price">The new price of the product.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> UpdateProductPrice(string username, string productName, decimal price);

    /// <summary>
    /// Updates the stock quantity of a product.
    /// </summary>
    /// <param name="username">The username requesting the update.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The new stock quantity of the product.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> UpdateProductStocks(string username, string productName, int quantity);

    /// <summary>
    /// Deletes a product from the inventory.
    /// </summary>
    /// <param name="requester">The requester of the product deletion.</param>
    /// <param name="productName">The name of the product to be deleted.</param>
    /// <returns>A result indicating success or an error type.</returns>
    Result<VoidResult, BaoErrorType> DeleteProduct(string requester, string productName);
}