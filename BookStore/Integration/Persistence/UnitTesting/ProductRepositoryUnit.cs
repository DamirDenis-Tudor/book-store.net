using Persistence.DAL;
using Persistence.DTO.Product;

namespace Integration.Persistence.UnitTesting;

public class ProductRepositoryUnit
{
    private readonly List<ProductDto> _productDtos = [];
    
    [SetUp]
    public void RegisterProductUnitTest()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        _productDtos.AddRange(new[]
        {
            new ProductDto { Name = "Lecture11",Description = "", Price = 10.0m, Quantity = 100, Category = "Books", Link = ".png"},
            new ProductDto { Name = "Lecture21",Description = "", Price = 15.0m, Quantity = 100, Category = "Books", Link = ".png"},
            new ProductDto { Name = "Laptop11",Description = "", Price = 20.0m, Quantity = 100, Category = "Laptops", Link = ".png"}
        });

        _productDtos.ToList().ForEach(p =>
                Assert.That(PersistenceFacade.Instance.ProductRepository?.RegisterProduct(p).IsSuccess, Is.EqualTo(true))
        );
    }

    [TearDown]
    public void DeleteProductUnitTest()
    {
        _productDtos.ToList().ForEach(p =>
            Assert.That(PersistenceFacade.Instance.ProductRepository?.DeleteProduct(p.Name).IsSuccess, Is.EqualTo(true))
        );
        _productDtos.Clear();
    }

    [Test]
    public void CreateCheckDeleteProductUnitTest()
    {
        Assert.That(PersistenceFacade.Instance.ProductRepository?.RegisterProduct(_productDtos[2]).IsSuccess, Is.EqualTo(false));

        Assert.That(PersistenceFacade.Instance.ProductRepository?.GetProduct(_productDtos[0].Name).IsSuccess, Is.EqualTo(true));
        
        PersistenceFacade.Instance.ProductRepository.GetCategories().SuccessValue.ToList().ForEach(Console.WriteLine);
    }

    [Test]
    public void UpdateUnitTest()
    {
        Assert.That(PersistenceFacade.Instance.ProductRepository?.UpdatePrice(_productDtos[1].Name, 1).IsSuccess, Is.EqualTo(true));
        
        var product = PersistenceFacade.Instance.ProductRepository.GetProduct(_productDtos[1].Name);
        Assert.That(product.IsSuccess, Is.EqualTo(true));
        Assert.That(product.SuccessValue.Price, Is.EqualTo(1));
        
        Assert.That(PersistenceFacade.Instance.ProductRepository.UpdateQuantity(_productDtos[1].Name, 1).IsSuccess, Is.EqualTo(true));
        
        product = PersistenceFacade.Instance.ProductRepository.GetProduct(_productDtos[1].Name);
        Assert.That(product.IsSuccess, Is.EqualTo(true));
        Assert.That(product.SuccessValue.Quantity, Is.EqualTo(1));
    }
}