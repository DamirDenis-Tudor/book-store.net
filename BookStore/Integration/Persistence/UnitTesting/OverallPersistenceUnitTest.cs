using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class OverallPersistenceUnitTest
{
    private readonly string _sessionCode = "qwerewbfcsd1231243drs233";

    private readonly UserRegisterDto _user = new()
    {
        FirstName = "test", LastName = "testCreateAndDelete", Username = "test_12345",
        Password = "testC", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
    };

    private readonly BillDto _billDto = new()
    {
        Address = "Tester cel Mare", Telephone = "1000000000",
        Country = "Testania", City = "Tity", PostalCode = "123456"
    };

    private readonly List<ProductDto> _products =
    [
        new ProductDto { Name = "Lecture1",Description = "", Price = 10.0m, Quantity = 100, Category = "Books", Link = ".png" },
        new ProductDto { Name = "Lecture2",Description = "", Price = 15.0m, Quantity = 100, Category = "Books", Link = ".png" },
        new ProductDto { Name = "Laptop1",Description = "", Price = 20.0m, Quantity = 100, Category = "Laptops", Link = ".png" }
    ];

    [SetUp]
    public void RegisterUnitTest()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        
        Assert.That(PersistenceFacade.Instance.UserRepository.RegisterUser(_user).IsSuccess, Is.EqualTo(true));
        Assert.That(
            PersistenceFacade.Instance.BillRepository.UpdateBillByUsername(_user.Username, _billDto).IsSuccess,
            Is.EqualTo(true)
        );
        _products.ForEach(p =>
            Assert.That(PersistenceFacade.Instance.ProductRepository.RegisterProduct(p).IsSuccess, Is.EqualTo(true)));

        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductName = p.Name,Description = "", SessionCode = _sessionCode, OrderQuantity = new Random().Next(100) })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = _sessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceFacade.Instance.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
    }

    [TearDown]
    public void DeleteUnitTest()
    {
        Assert.That(PersistenceFacade.Instance.UserRepository.DeleteUser(_user.Username).IsSuccess, Is.EqualTo(true));
        _products.ForEach(p => PersistenceFacade.Instance.ProductRepository.DeleteProduct(p.Name));
    }

    [Test]
    public void CheckOverallUnitTest()
    {
        var productStats = PersistenceFacade.Instance.ProductRepository.GetAllProductsStats();
        Assert.That(productStats.IsSuccess, Is.EqualTo(true));
        productStats.SuccessValue.ToList().ForEach(Console.WriteLine);

        var userOrders = PersistenceFacade.Instance.OrderRepository.GetAllOrdersByUsername(_user.Username);
        Assert.That(userOrders.IsSuccess, Is.EqualTo(true));
        userOrders.SuccessValue.ToList().ForEach(Console.WriteLine);
    }
}