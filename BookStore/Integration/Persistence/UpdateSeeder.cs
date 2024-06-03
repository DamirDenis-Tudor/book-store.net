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

public class UpdateSeeder
{
    private sealed record UserBill
    {
        public required UserRegisterDto UserRegisterDto { get; set; }
        public required BillDto BillDto { get; set; }
    }
    
    public void PrepareDatabase(IntegrationMode integrationMode) =>
        PersistenceFacade.Instance.SetIntegrationMode(integrationMode);
    
    [OneTimeSetUp]
    public void SetUp()=>
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);
    
    public void AddUsers()
    {
        Console.WriteLine(DateTime.Now);
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/Persistence/Resources/UsersSeed.json".Replace('/',
                Path.DirectorySeparatorChar));
        JsonSerializer.Deserialize<List<UserBill>>(file)?.ForEach(userBill =>
            {
                userBill.UserRegisterDto = GdprMapper.DoUserInfoDtoGdpr(userBill.UserRegisterDto);
                userBill.BillDto = GdprMapper.DoBillGdpr(userBill.BillDto, userBill.UserRegisterDto.Password);
                Assert.That(PersistenceFacade.Instance.UserRepository
                    .RegisterUser(userBill.UserRegisterDto).IsSuccess, Is.EqualTo(true));
                Assert.That(PersistenceFacade.Instance.BillRepository
                        .UpdateBillByUsername(userBill.UserRegisterDto.Username, userBill.BillDto).IsSuccess,
                    Is.EqualTo(true));
            }
        );
    }
    
    [Test, Order(1)]
    public void AddProducts()
    {
        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/Persistence/Resources/ProductsSeed.json".Replace('/',
                Path.DirectorySeparatorChar));
        JsonSerializer.Deserialize<List<ProductDto>>(file)?.ForEach(productDto =>
            PersistenceFacade.Instance.ProductRepository.RegisterProduct(productDto)
        );
    }
    
    public void AddOrders()
    {
        var persistence = PersistenceFacade.Instance;

        var file = File.ReadAllText(
            $"{SlnDirectory.GetPath()}/Integration/Persistence/Resources/OrdersSeed.json".Replace('/',
                Path.DirectorySeparatorChar));

        JsonSerializer.Deserialize<List<OrderBto>>(file)?.ForEach(orderBto =>
            {
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
                            SessionCode = orderSession.SessionCode,
                            OrderQuantity = item.OrderQuantity,
                            Price = product.SuccessValue.Price * item.OrderQuantity,
                            ProductInfoDto = product.SuccessValue.ProductInfoDto
                        });
                        orderSession.TotalPrice += product.SuccessValue.Price * item.OrderQuantity;
                    }
                );

                persistence.OrderRepository.RegisterOrderSession(orderSession);
            }
        );
    }
}