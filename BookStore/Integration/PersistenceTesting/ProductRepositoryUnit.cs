using Persistence.DAL;
using Persistence.DTO.Product;

namespace UnitTesting.PersistenceTesting;

public class ProductRepositoryUnit
{
    private readonly List<ProductDto> _productDtos = [];
    
    [SetUp]
    public void RegisterProductUnitTest()
    {
        PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Testing);
        _productDtos.AddRange(new[]
        {
            new ProductDto { Name = "Lecture1",Description = "", Price = 10.0m, Quantity = 100, Category = "Books", Link = ".png"},
            new ProductDto { Name = "Lecture2",Description = "", Price = 15.0m, Quantity = 100, Category = "Books", Link = ".png"},
            new ProductDto { Name = "Laptop1",Description = "", Price = 20.0m, Quantity = 100, Category = "Laptops", Link = ".png"}
        });

        _productDtos.ToList().ForEach(p =>
                Assert.That(PersistenceAccess.Instance.ProductRepository?.RegisterProduct(p).IsSuccess, Is.EqualTo(true))
        );
    }

    [TearDown]
    public void DeleteProductUnitTest()
    {
        _productDtos.ToList().ForEach(p =>
            Assert.That(PersistenceAccess.Instance.ProductRepository?.DeleteProduct(p.Name).IsSuccess, Is.EqualTo(true))
        );
        _productDtos.Clear();
    }

    [Test]
    public void CreateCheckDeleteProductUnitTest()
    {
        Assert.That(PersistenceAccess.Instance.ProductRepository?.RegisterProduct(_productDtos[2]).IsSuccess, Is.EqualTo(false));

        Assert.That(PersistenceAccess.Instance.ProductRepository?.GetProduct(_productDtos[0].Name).IsSuccess, Is.EqualTo(true));

        var allProducts = PersistenceAccess.Instance.ProductRepository.GetAllProducts();
        Assert.That(allProducts.IsSuccess, Is.EqualTo(true));
        allProducts.SuccessValue.ToList().ForEach(Console.WriteLine);
        Assert.That(allProducts.SuccessValue.Count, Is.EqualTo(3));
        
        var allProductsByCategory = PersistenceAccess.Instance.ProductRepository.GetAllProductsByCategory("Books");
        Assert.That(allProductsByCategory.IsSuccess, Is.EqualTo(true));
        allProductsByCategory.SuccessValue.ToList().ForEach(Console.WriteLine);
        Assert.That(allProductsByCategory.SuccessValue.Count, Is.EqualTo(2));

        var allProductsStats = PersistenceAccess.Instance.ProductRepository.GetAllProductsStats();
        Assert.That(allProductsStats.IsSuccess, Is.EqualTo(true));
        allProductsStats.SuccessValue.ToList().ForEach(Console.WriteLine);
        Assert.That(allProductsStats.SuccessValue.Count, Is.EqualTo(3));
        
        PersistenceAccess.Instance.ProductRepository.GetCategories().SuccessValue.ToList().ForEach(Console.WriteLine);
    }

    [Test]
    public void UpdateUnitTest()
    {
        Assert.That(PersistenceAccess.Instance.ProductRepository?.UpdatePrice(_productDtos[1].Name, 1).IsSuccess, Is.EqualTo(true));
        
        var product = PersistenceAccess.Instance.ProductRepository.GetProduct(_productDtos[1].Name);
        Assert.That(product.IsSuccess, Is.EqualTo(true));
        Assert.That(product.SuccessValue.Price, Is.EqualTo(1));
        
        Assert.That(PersistenceAccess.Instance.ProductRepository.UpdateQuantity(_productDtos[1].Name, 1).IsSuccess, Is.EqualTo(true));
        
        product = PersistenceAccess.Instance.ProductRepository.GetProduct(_productDtos[1].Name);
        Assert.That(product.IsSuccess, Is.EqualTo(true));
        Assert.That(product.SuccessValue.Quantity, Is.EqualTo(1));
    }
}