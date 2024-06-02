using Business.BAL;
using Business.Mappers;
using Persistence.DAL;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Business.UnitTesting;

public class InventoryService
{
    private BusinessFacade _businessFacade;
    private ProductDto _productDto;
    private UserRegisterDto _clientUser;
    private UserRegisterDto _providerUser;
    private UserRegisterDto _adminUser;

    [OneTimeSetUp]
    public void PrepareDatabase()
    {
        PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Testing);
        _businessFacade = BusinessFacade.Instance;


        _adminUser = new UserRegisterDto
        {
            FirstName = "AdminInventory", LastName = "AdminInventory", Username = "adminInventory_123",
            Password = "adminInventory_123", Email = "admin@admin.com", UserType = "ADMIN"
        };
        _productDto = new ProductDto
        {
            Price = 0,
            Quantity = 0,
            ProductInfoDto = new ProductInfoDto
            {
                Category = "TEST",
                Name = "null",
                Description = "mama"
            }
        };
        var gdprAdmin = GdprMapper.DoUserInfoDtoGdpr(_adminUser);
        Assert.That(PersistenceFacade.Instance.UserRepository.RegisterUser(gdprAdmin).IsSuccess, Is.EqualTo(true));

        _clientUser = new UserRegisterDto
        {
            FirstName = "ClientInventory", LastName = "ClientInventory", Username = "clientInventory_123",
            Password = "clientInventory_123", Email = "client@client.com", UserType = "CLIENT"
        };
        Assert.That(_businessFacade.UsersService.RegisterClient(_clientUser).IsSuccess, Is.EqualTo(true));

        _providerUser = new UserRegisterDto
        {
            FirstName = "ProviderInventory", LastName = "ProviderInventory", Username = "providerInventory_123",
            Password = "providerInventory_123", Email = "provider@provider.com", UserType = "PROVIDER"
        };
        Assert.That(_businessFacade.UsersService.RegisterProvider(_adminUser.Username, _providerUser).IsSuccess,
            Is.EqualTo(true));
    }

    [Test, Order(1)]
    public void AllowedToRegisterProduct()
    {
        var register = _businessFacade.InventoryService.RegisterProduct(_providerUser.Username, _productDto);

        Assert.That(register.IsSuccess, Is.EqualTo(true));
    }

    [Test, Order(2)]
    public void UnalloyedRegisterProduct()
    {
        Assert.That(_businessFacade.InventoryService.RegisterProduct(_clientUser.Username, _productDto).IsSuccess,
            Is.EqualTo(false));
    }

    [Test, Order(3)]
    public void UnalloyedDeleteProduct()
    {
        Assert.That(
            _businessFacade.InventoryService.DeleteProduct(_clientUser.Username, _productDto.ProductInfoDto.Name)
                .IsSuccess, Is.EqualTo(false));
    }

    [Test, Order(4)]
    public void AllowedToDeleteProduct()
    {
        var delete =
            _businessFacade.InventoryService.DeleteProduct(_providerUser.Username, _productDto.ProductInfoDto.Name);

        Assert.That(delete.IsSuccess, Is.EqualTo(true));
    }
}