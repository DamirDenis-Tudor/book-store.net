using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class OrderRepositoryUnit
{
    private const string SessionCode = "code_123";

    private readonly UserRegisterDto _user = new()
    {
        FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
        Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
    };

    private readonly List<ProductDto> _products =
    [
        new ProductDto
        {
            ProductInfoDto = new ProductInfoDto { Name = "Lecture1", Description = "TEST", Category = "Books" },
            Price = 10.0m, Quantity = 100,
        },
        new ProductDto
        {
            ProductInfoDto = new ProductInfoDto { Name = "Lecture2", Description = "TEST", Category = "Books" },
            Price = 15.0m, Quantity = 100,
        },
        new ProductDto
        {
            ProductInfoDto = new ProductInfoDto { Name = "Lecture3", Description = "TEST", Category = "Laptops" },
            Price = 20.0m, Quantity = 100,
        }
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
        _products.ForEach(p => PersistenceFacade.Instance.ProductRepository.DeleteProduct(p.ProductInfoDto.Name));
        PersistenceFacade.Instance.UserRepository.DeleteUser(_user.Username);
    }

    [Test]
    public void RegisterAndDeleteUnitTest()
    {
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductInfoDto = p.ProductInfoDto, SessionCode = SessionCode, OrderQuantity = 5 })
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
        Assert.That(
            PersistenceFacade.Instance.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
    }

    [Test]
    public void GettersUnitTest()
    {
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductInfoDto = p.ProductInfoDto, SessionCode = SessionCode, OrderQuantity = 5 })
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
        Assert.That(
            PersistenceFacade.Instance.OrderRepository.DeleteOrderSession(orderSessionDto.SessionCode).IsSuccess,
            Is.EqualTo(true));
    }
}