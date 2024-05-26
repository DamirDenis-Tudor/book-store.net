using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

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
        new ProductDto { Name = "Lecture1",Description = "",Price = 10.0m, Quantity = 100, Category = "Books", Link = ".png"},
        new ProductDto { Name = "Lecture2",Description = "",Price = 15.0m, Quantity = 100, Category = "Books", Link = ".png" },
        new ProductDto { Name = "Laptop1",Description = "",Price = 20.0m, Quantity = 100, Category = "Laptops", Link = ".png" }
    ];

    [SetUp]
    public void RegisterOrderUnitTest()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        Console.WriteLine("tessssss");
        PersistenceFacade.Instance.UserRepository.RegisterUser(_user);
        _products.ForEach(p => PersistenceFacade.Instance.ProductRepository.RegisterProduct(p));
    }

    [TearDown]
    public void DeleteOrderUnitTest()
    {
        _products.ForEach(p => PersistenceFacade.Instance.ProductRepository.DeleteProduct(p.Name));
        PersistenceFacade.Instance.UserRepository.DeleteUser(_user.Username);
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

        Assert.That(PersistenceFacade.Instance.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceFacade.Instance.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
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

        Assert.That(PersistenceFacade.Instance.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceFacade.Instance.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));

        Assert.That(PersistenceFacade.Instance.OrderRepository.GetSessionOrder(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
        Assert.That(PersistenceFacade.Instance.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
    }
}