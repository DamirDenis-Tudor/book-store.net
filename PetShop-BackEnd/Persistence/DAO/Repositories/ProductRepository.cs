using System.Data.Common;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.DTO.Product;


namespace Persistence.DAO.Repositories;

internal class ProductRepository(PersistenceAccess.DatabaseContext dbContext) : IProductRepository
{
    private readonly ILogger _logger = Logger.Logger.Instance.GetLogger<BillRepository>();

    public bool RegisterProduct(ProductDto productDto)
    {
        try
        {
            dbContext.Products.Add(MapperDto.MapToProduct(productDto));
            dbContext.SaveChanges();
        }
        catch (DbException e)
        {
            _logger.LogError(e.ToString());
            return false;
        }

        return true;
    }

    public ProductDto? GetProduct(string name) =>
        MapperDto.MapToProductDto(dbContext.Products.FirstOrDefault(p => p.Name == name));


    public List<ProductDto?> GetAllProducts()
    {
        var products = new List<ProductDto?>();
        dbContext.Products.ToList().ForEach(p => products.Add(MapperDto.MapToProductDto(p)));
        return products;
    }

    public List<ProductStatsDto?> GetAllProductsStats()
    {
        var products = new List<ProductStatsDto?>();
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
                        Name = p.Name,
                        TotalRevenue = totalRevenue,
                        TotalItemsSold = totalItemsSold,
                        Photo = p.Photo
                    });
                });
        return products;
    }

    public bool UpdatePrice(string name, int newPrice)
    {
        try
        {
            var existingProduct = dbContext.Products.FirstOrDefault(p => p.Name == name);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product {} not registered", name);
                return false;
            }

            existingProduct.Price = newPrice;
            dbContext.Update(existingProduct);
            dbContext.SaveChanges();
        }
        catch (DbException e)
        {
            _logger.LogError(e.ToString());
            return false;
        }

        return true;
    }

    public bool UpdateQuantity(string name, int quantity)
    {
        try
        {
            var existingProduct = dbContext.Products.FirstOrDefault(p => p.Name == name);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product {} not registered", name);
                return false;
            }

            existingProduct.Quantity = quantity;

            dbContext.SaveChanges();
        }
        catch (DbException e)
        {
            _logger.LogError(e.ToString());
            return false;
        }

        return true;
    }
}