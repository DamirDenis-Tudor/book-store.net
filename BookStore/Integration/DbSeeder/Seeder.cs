using System.Text.Json;
using Business.DTO;
using Common;
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.DbSeeder;

public class Seeder
{
    private sealed record UserBill
    {
        public required UserInfoDto UserInfoDto { get; init; }
        public required BillDto BillDto { get; init; }
    }

    [OneTimeSetUp]
    public void PrepareDatabase() => PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Integration);

    [Test, Order(1)]
    public void AddUsers()
    {
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/DbSeeder/Resources/UsersSeed.json");
        JsonSerializer.Deserialize<List<UserBill>>(file)?.ForEach(userBill =>
            {
                Assert.That(PersistenceAccess.Instance.UserRepository
                    .RegisterUser(userBill.UserInfoDto).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceAccess.Instance.BillRepository
                        .UpdateBillToUsername(userBill.UserInfoDto.Username, userBill.BillDto).IsSuccess,
                    Is.EqualTo(true));
            }
        );
    }

    [Test, Order(2)]
    public void AddProducts()
    {
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/DbSeeder/Resources/ProductsSeed.json");
        JsonSerializer.Deserialize<List<ProductDto>>(file)?.ForEach(productDto =>
            Assert.That(PersistenceAccess.Instance.ProductRepository.RegisterProduct(productDto).IsSuccess,
                Is.EqualTo(true))
        );
    }

    [Test, Order(3)]
    public void AddOrders()
    {
        var persistence = PersistenceAccess.Instance;

        var file = File.ReadAllText($"{SlnDirectory.GetPath()}/Integration/DbSeeder/Resources/OrdersSeed.json");

        JsonSerializer.Deserialize<List<OrderDto>>(file)?.ForEach(orderDto =>
            {
                var orderSession = new OrderSessionDto
                {
                    Username = orderDto.Username,
                    SessionCode = new Random().Next(100000).ToString(),
                    Status = "Comanda plasata",
                    OrderProducts = []
                };

                orderDto.OrderItemDtos.ForEach(item =>
                    {
                        var product = persistence.ProductRepository.GetProduct(item.ProductName);
                        if (!product.IsSuccess) return;
                        orderSession.OrderProducts.Add(new OrderProductDto
                        {
                            ProductName = product.SuccessValue.Name,
                            Description = product.SuccessValue.Description,
                            SessionCode = orderSession.SessionCode,
                            OrderQuantity = item.OrderQuantity,
                            Price = product.SuccessValue.Price,
                            Link = product.SuccessValue.Link
                        });
                        orderSession.TotalPrice += product.SuccessValue.Price * item.OrderQuantity;
                    }
                );

                persistence.OrderRepository.RegisterOrderSession(orderSession);
            }
        );
    }
}