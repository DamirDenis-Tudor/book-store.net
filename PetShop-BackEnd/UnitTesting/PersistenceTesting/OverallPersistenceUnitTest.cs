using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.PersistenceTesting;

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
    
    private readonly List<ProductDto> _products = [
        new ProductDto { Name = "Book", Price = 10.0m, Quantity = 100 },
        new ProductDto { Name = "Food", Price = 15.0m, Quantity = 100 },
        new ProductDto { Name = "Toy", Price = 20.0m, Quantity = 100 }
    ];

    [SetUp]
    public void RegisterUnitTest()
    {
        Assert.That(PersistenceAccess.UserRepository.RegisterUser(_user).IsSuccess, Is.EqualTo(true));
        Assert.That(
            PersistenceAccess.BillRepository.UpdateBillToUsername(_user.Username, _billDto).IsSuccess,
            Is.EqualTo(true)
        );
        _products.ForEach(p => Assert.That(PersistenceAccess.ProductRepository.RegisterProduct(p).IsSuccess, Is.EqualTo(true)));
        
        List<OrderProductDto> orderProductDtos = [];
        _products.ForEach(p => orderProductDtos.Add(new OrderProductDto
            { ProductName = p.Name, SessionCode = _sessionCode, Quantity = new Random().Next(100) })
        );

        var orderSessionDto = new OrderSessionDto
        {
            Username = _user.Username,
            SessionCode = _sessionCode,
            Status = "Paid",
            OrderProducts = orderProductDtos
        };

        Assert.That(PersistenceAccess.OrderRepository.RegisterOrderSession(orderSessionDto).IsSuccess, Is.EqualTo(true));
    }

    [TearDown]
    public void DeleteUnitTest()
    {
        Assert.That(PersistenceAccess.UserRepository.DeleteUser(_user.Username).IsSuccess, Is.EqualTo(true));
        _products.ForEach(p => PersistenceAccess.ProductRepository.DeleteProduct(p.Name));  
    }

    [Test]
    public void CheckOverallUnitTest()
    {
        var productStats = PersistenceAccess.ProductRepository.GetAllProductsStats();
        Assert.That(productStats.IsSuccess, Is.EqualTo(true));
        productStats.SuccessValue.ToList().ForEach(Console.WriteLine);
        
        var userOrders = PersistenceAccess.OrderRepository.GetAllOrdersByUsername(_user.Username);
        Assert.That(userOrders.IsSuccess, Is.EqualTo(true));
        userOrders.SuccessValue.ToList().ForEach(Console.WriteLine);
    }
}