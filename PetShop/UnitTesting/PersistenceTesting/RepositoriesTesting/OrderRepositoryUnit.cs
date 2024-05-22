using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.PersistenceTesting.RepositoriesTesting;

public class OrderRepositoryUnit
{
    private const string SessionCode = "code_123";

    private readonly UserInfoDto _user = new()
    {
        FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
        Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
    };

    private readonly List<ProductDto> _products =
    [
        new ProductDto { Name = "Lecture1",Description = "",Price = 10.0m, Quantity = 100, Category = "Books" },
        new ProductDto { Name = "Lecture2",Description = "",Price = 15.0m, Quantity = 100, Category = "Books" },
        new ProductDto { Name = "Laptop1",Description = "",Price = 20.0m, Quantity = 100, Category = "Laptops" }
    ];

    [SetUp]
    public void RegisterOrderUnitTest()
    {
        PersistenceAccess.SetIntegrationMode(IntegrationMode.Testing);

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
            { ProductName = p.Name,Description = "",SessionCode = SessionCode, OrderQuantity = 5 })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = SessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceAccess.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
    }

    [Test]
    public void GettersUnitTest()
    {
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductName = p.Name, Description = "", SessionCode = SessionCode, OrderQuantity = 5 })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = SessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceAccess.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));

        var sessionOrders = PersistenceAccess.OrderRepository.GetAllOrders();
        Assert.That(sessionOrders.IsSuccess, Is.EqualTo(true));
        Assert.That(sessionOrders.SuccessValue.Count, Is.EqualTo(1));

        sessionOrders.SuccessValue.ToList()[0].OrderProducts.ForEach(Console.WriteLine);

        Assert.That(PersistenceAccess.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceAccess.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
    }
}