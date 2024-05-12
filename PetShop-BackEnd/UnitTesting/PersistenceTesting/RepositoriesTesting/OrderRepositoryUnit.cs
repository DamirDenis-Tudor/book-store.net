using NUnit.Framework.Interfaces;
using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting;

public class OrderRepositoryUnit
{
    private string sessionCode = "code_123";

    private UserInfoDto _user = new UserInfoDto
    {
        FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
        Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
    };

    private List<ProductDto> _products = [
        new ProductDto { Name = "Book", Price = 10.0m, Quantity = 100 },
        new ProductDto { Name = "Food", Price = 15.0m, Quantity = 100 },
        new ProductDto { Name = "Toy", Price = 20.0m, Quantity = 100 }
    ];

    [SetUp]
    public void RegisterOrderUnitTest()
    {
        PersistenceAccess.UserRepository.RegisterUser(_user);
        _products.ForEach(p => PersistenceAccess.ProductRepository.RegisterProduct(p));
    }

    [TearDown]
    public void DeleteOrderUnitTest()
    {
        _products.ForEach(p => PersistenceAccess.ProductRepository.DeleteProduct(p.Name));        
        PersistenceAccess.UserRepository.DeleteUser(_user.Username);
    }

    [Test]
    public void RegisterAndDeleteUnitTest()
    {
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductName = p.Name, SessionCode = sessionCode, Quantity = 5 })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = sessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceAccess.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess, Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess, Is.EqualTo(true));
    }
    
    [Test]
    public void GettersUnitTest()
    {
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductName = p.Name, SessionCode = sessionCode, Quantity = 5 })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = sessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceAccess.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess, Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess, Is.EqualTo(true));
        
        var sessionOrders = PersistenceAccess.OrderRepository.GetAllOrders();
        Assert.That(sessionOrders.IsSuccess, Is.EqualTo(true));
        Assert.That(sessionOrders.SuccessValue.Count, Is.EqualTo(1));
        
        sessionOrders.SuccessValue.ToList()[0].OrderProducts.ForEach(Console.WriteLine);
        
        Assert.That(PersistenceAccess.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess, Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess, Is.EqualTo(true));
    }
}