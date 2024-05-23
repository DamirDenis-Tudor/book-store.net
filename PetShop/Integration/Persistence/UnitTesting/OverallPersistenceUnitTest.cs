using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.UnitTesting;

public class OverallPersistenceUnitTest
{
    private readonly string _sessionCode = "qwerewbfcsd1231243drs233";

    private readonly UserInfoDto _user = new()
    {
        FirstName = "testCreateAndDelete", LastName = "testCreateAndDelete", Username = "test_12345CreateAndDelete",
        Password = "testCreateAndDelete", Email = "test@test.testCreateAndDelete", UserType = "TESTER"
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
        PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Testing);
        
        Assert.That(PersistenceAccess.Instance.UserRepository.RegisterUser(_user).IsSuccess, Is.EqualTo(true));
        Assert.That(
            PersistenceAccess.Instance.BillRepository.UpdateBillToUsername(_user.Username, _billDto).IsSuccess,
            Is.EqualTo(true)
        );
        _products.ForEach(p =>
            Assert.That(PersistenceAccess.Instance.ProductRepository.RegisterProduct(p).IsSuccess, Is.EqualTo(true)));

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

        Assert.That(PersistenceAccess.Instance.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess,
            Is.EqualTo(true));
    }

    [TearDown]
    public void DeleteUnitTest()
    {
        Assert.That(PersistenceAccess.Instance.UserRepository.DeleteUser(_user.Username).IsSuccess, Is.EqualTo(true));
        _products.ForEach(p => PersistenceAccess.Instance.ProductRepository.DeleteProduct(p.Name));
    }

    [Test]
    public void CheckOverallUnitTest()
    {
        var productStats = PersistenceAccess.Instance.ProductRepository.GetAllProductsStats();
        Assert.That(productStats.IsSuccess, Is.EqualTo(true));
        productStats.SuccessValue.ToList().ForEach(Console.WriteLine);

        var userOrders = PersistenceAccess.Instance.OrderRepository.GetAllOrdersByUsername(_user.Username);
        Assert.That(userOrders.IsSuccess, Is.EqualTo(true));
        userOrders.SuccessValue.ToList().ForEach(Console.WriteLine);
    }
}