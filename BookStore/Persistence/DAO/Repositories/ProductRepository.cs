/**************************************************************************
 *                                                                        *
 *  Description: ProductRepository                                        *
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
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO.Product;
using Persistence.Mappers;


namespace Persistence.DAO.Repositories;

internal class ProductRepository(DatabaseContext dbContext) : IProductRepository
{
    public Result<VoidResult, DaoErrorType> RegisterProduct(ProductDto productDto)
    {
        try
        {
            if (GetProduct(productDto.Name).IsSuccess)
                return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.AlreadyRegistered,
                    $"Product {productDto.Name} already registered.");
            dbContext.Products.Add(MapperDto.MapToProduct(productDto));
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.DatabaseError,
                $"Failed to register product {productDto.Name}: {e.Message}");
        }

        return Result<VoidResult, DaoErrorType>.Success(VoidResult.Get(), $"Product {productDto.Name} registered successfully.");
    }

    public Result<IList<string>, DaoErrorType> GetCategories()
    {
        var categories = new List<string>();
        dbContext.Products.ToList().ForEach(product => { categories.Add(product.Category); });

        return categories.Count != 0
            ? Result<IList<string>, DaoErrorType>.Success(categories.Distinct().ToList(),
                "Successfully fetched categories.")
            : Result<IList<string>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "Fail to fetch categories.");
    }

    public Result<ProductDto, DaoErrorType> GetProduct(string name)
    {
        var productDto = MapperDto.MapToProductDto(dbContext.Products.FirstOrDefault(p => p.Name == name));
        if (productDto == null)
            return Result<ProductDto, DaoErrorType>
                .Fail(DaoErrorType.NotFound, $"Product {name} not found.");
        return Result<ProductDto, DaoErrorType>
            .Success(productDto, $"Product {name} found.");
    }


    public Result<IList<ProductDto>, DaoErrorType> GetAllProducts()
    {
        var products = new List<ProductDto>();
        
		dbContext.Products.ToList().ForEach(p =>
        {
            dbContext.Entry(p).Reload();
            products.Add(MapperDto.MapToProductDto(p)!);
        });
        
        return products.Count != 0
            ? Result<IList<ProductDto>, DaoErrorType>.Success(products, "Products registered.")
            : Result<IList<ProductDto>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "No products registered.");
    }

    public Result<IList<ProductDto>, DaoErrorType> GetAllProductsByCategory(string category)
    {
        var orderSessions = dbContext.Products
            .Where(p => p.Category == category)
            .ToList();

        var orderSessionDtos = orderSessions.Select(MapperDto.MapToProductDto).ToList();

        return orderSessions.Count != 0 && orderSessionDtos.Count != 0
            ? Result<IList<ProductDto>, DaoErrorType>.Success(orderSessionDtos!, "Products list returned.")
            : Result<IList<ProductDto>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "No order session found.");
    }

    public Result<IList<ProductStatsDto>, DaoErrorType> GetAllProductsStats()
    {
        var products = new List<ProductStatsDto>();
        dbContext.Products.Include(p => p.OrderProducts).ToList()
            .ForEach(
                p =>
                {
                    var totalRevenue = 0.0m;
                    var totalItemsSold = 0;
                    p.OrderProducts?.ToList()
                        .ForEach(
                            o =>
                            {
                                totalRevenue += p.Price * o.Quantity;
                                totalItemsSold += o.Quantity;
                            }
                        );
                    products.Add(new ProductStatsDto
                    {
                        TotalRevenue = totalRevenue,
                        TotalItemsSold = totalItemsSold,
                        ProductDto = MapperDto.MapToProductDto(p)!
                    });
                });

        return Result<IList<ProductStatsDto>, DaoErrorType>
            .Success(products, "Products found.");
    }

    public Result<VoidResult, DaoErrorType> UpdatePrice(string name, decimal newPrice)
    {
        var existingProduct = dbContext.Products.FirstOrDefault(p => p.Name == name);
        if (existingProduct == null)
        {
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.NotFound, $"Product '{name}' not found.");
        }

        existingProduct.Price = newPrice;
        try
        {
            dbContext.Update(existingProduct);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.DatabaseError,
                $"Failed to update product price: {e.Message}");
        }

        return Result<VoidResult, DaoErrorType>.Success(VoidResult.Get(),
            $"Product '{name}' price updated successfully.");
    }

    public Result<VoidResult, DaoErrorType> UpdateQuantity(string name, int quantity)
    {
        var existingProduct = dbContext.Products.FirstOrDefault(p => p.Name == name);
        if (existingProduct == null)
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.NotFound, $"Product '{name}' not found.");

        existingProduct.Quantity = quantity;
        try
        {
            dbContext.Update(existingProduct);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.DatabaseError,
                $"Failed to update product quantity: {e.Message}");
        }

        return Result<VoidResult, DaoErrorType>.Success(VoidResult.Get(),
            $"Product '{name}' quantity updated successfully.");
    }

    public Result<VoidResult, DaoErrorType> DeleteProduct(string name)
    {
        try
        {
            var existingProduct = dbContext.Products.FirstOrDefault(p => p.Name == name);
            if (existingProduct == null)
                return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.NotFound, $"Product '{name}' not found.");

            dbContext.Products.Remove(existingProduct);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(DaoErrorType.DatabaseError,
                $"Failed to delete product '{name}'.");
        }

        return Result<VoidResult, DaoErrorType>.Success(VoidResult.Get(), $"Product '{name}' deleted successfully.");
    }
}