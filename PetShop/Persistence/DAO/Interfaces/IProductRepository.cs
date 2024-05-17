/**************************************************************************
 *                                                                        *
 *  Description: IProductRepository                                       *
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

using Logger;
using Persistence.DTO.Product;

namespace Persistence.DAO.Interfaces;

/// <summary>
/// Interface for Product Repository operations.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Registers a new product.
    /// </summary>
    /// <param name="productDto">The product data transfer object containing the details of the product to register.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<bool, DaoErrorType> RegisterProduct(ProductDto productDto);

    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="name">The name of the product to retrieve.</param>
    /// <returns>A Result containing either the ProductDto or a DaoErrorType indicating the type of error.</returns>
    Result<ProductDto, DaoErrorType> GetProduct(string name);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A Result containing either a list of ProductDto or a DaoErrorType indicating the type of error.</returns>
    Result<IList<ProductDto>, DaoErrorType> GetAllProducts();

    /// <summary>
    /// Retrieves all products by category.
    /// </summary>
    /// <param name="category">The category of products to retrieve.</param>
    /// <returns>A Result containing either a list of ProductDto or a DaoErrorType indicating the type of error.</returns>
    Result<IList<ProductDto>, DaoErrorType> GetAllProductsByCategory(string category);

    /// <summary>
    /// Retrieves statistics for all products.
    /// </summary>
    /// <returns>A Result containing either a list of ProductStatsDto or a DaoErrorType indicating the type of error.</returns>
    Result<IList<ProductStatsDto>, DaoErrorType> GetAllProductsStats();

    /// <summary>
    /// Updates the price of a product.
    /// </summary>
    /// <param name="name">The name of the product to update.</param>
    /// <param name="newPrice">The new price of the product.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<bool, DaoErrorType> UpdatePrice(string name, int newPrice);

    /// <summary>
    /// Updates the quantity of a product.
    /// </summary>
    /// <param name="name">The name of the product to update.</param>
    /// <param name="quantity">The new quantity of the product.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<bool, DaoErrorType> UpdateQuantity(string name, int quantity);

    /// <summary>
    /// Deletes a product by its name.
    /// </summary>
    /// <param name="name">The name of the product to delete.</param>
    /// <returns>A Result containing either a boolean indicating success or a DaoErrorType indicating the type of error.</returns>
    Result<bool, DaoErrorType> DeleteProduct(string name);
}