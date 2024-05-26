using System.Text.Json;
using Business.BTO;
using Business.Mappers;
using Common;
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence;

public class Seeder
{
    private sealed record UserBill
    {
        public required UserInfoDto UserInfoDto { get; set; }
        public required BillDto BillDto { get; set; }
    }

    [OneTimeSetUp]
    public void PrepareDatabase() => PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Integration);

    [Test, Order(1)]
    public void AddUsers()
    {
        Console.WriteLine(DateTime.Now);
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}{Path.DirectorySeparatorChar}Integration/Persistence/Resources/UsersSeed.json".Replace('/', Path.DirectorySeparatorChar));
        JsonSerializer.Deserialize<List<UserBill>>(file)?.ForEach(userBill =>
            {
                userBill.UserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userBill.UserInfoDto);
                userBill.BillDto = GdprMapper.DoBillGdpr(userBill.BillDto);
                Assert.That(PersistenceFacade.Instance.UserRepository
                    .RegisterUser(userBill.UserInfoDto).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceFacade.Instance.BillRepository
                        .UpdateBillByUsername(userBill.UserInfoDto.Username, userBill.BillDto).IsSuccess,
                    Is.EqualTo(true));
            }
        );
    }

    [Test, Order(2)]
    public void AddProducts()
    {
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/Persistence/Resources/ProductsSeed.json".Replace('/', Path.DirectorySeparatorChar));
        JsonSerializer.Deserialize<List<ProductDto>>(file)?.ForEach(productDto =>
            Assert.That(PersistenceFacade.Instance.ProductRepository.RegisterProduct(productDto).IsSuccess,
                Is.EqualTo(true))
        );
    }

    [Test, Order(3)]
    public void AddOrders()
    {
        var persistence = PersistenceFacade.Instance;

        var file = File.ReadAllText($"{SlnDirectory.GetPath()}/Integration/Persistence/Resources/OrdersSeed.json".Replace('/', Path.DirectorySeparatorChar));

        JsonSerializer.Deserialize<List<OrderBto>>(file)?.ForEach(orderBto =>
            {
                orderBto = GdprMapper.DoOrderBto(orderBto);
                var orderSession = new OrderSessionDto
                {
                    Username = orderBto.Username,
                    SessionCode = new Random().Next(100000).ToString(),
                    Status = "Comanda plasata",
                    OrderProducts = []
                };

                orderBto.OrderItemBtos.ForEach(item =>
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