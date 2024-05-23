using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Product;
using Persistence.DTO.User;

namespace UnitTesting.Persistence.DbSeeder;

public class Seeder
{
    
    private void AddUsers()
    {
        var user = new UserInfoDto
        {
            FirstName = "Cristi", LastName = "Achirei", Username = "cristi_12345",
            Password = "parola_mea", Email = "cristi.achirei@gmail.com", UserType = "CLIENT",
        };
        
        Assert.That(PersistenceAccess.Instance.UserRepository.RegisterUser(user).IsSuccess, Is.EqualTo(true));
        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.Instance.BillRepository.UpdateBillToUsername(user.Username, billDto).IsSuccess, Is.EqualTo(true));
    }
    
    private void AddProviders()
    {
        var user = new UserInfoDto
        {
            FirstName = "Marius", LastName = "Bula", Username = "marius_12345",
            Password = "parola_mea", Email = "marius.bula@gmail.com", UserType = "PROVIDER",
        };
        
        Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));
        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.Instance.BillRepository?.UpdateBillToUsername(user.Username, billDto).IsSuccess, Is.EqualTo(true));
    }
    
    private void AddAdmins()
    {
        var user = new UserInfoDto
        {
            FirstName = "Andrei", LastName = "Pacala", Username = "andrei_12345",
            Password = "parola_mea", Email = "andrei.pacala@gmail.com", UserType = "ADMIN",
        };
        
        Assert.That(PersistenceAccess.Instance.UserRepository?.RegisterUser(user).IsSuccess, Is.EqualTo(true));
        var billDto = new BillDto
        {
            Address = "Tester cel Mare", Telephone = "1000000000",
            Country = "Testania", City = "Tity", PostalCode = "123456"
        };
        Assert.That(PersistenceAccess.Instance.BillRepository?.UpdateBillToUsername(user.Username, billDto).IsSuccess, Is.EqualTo(true));
    }
    
    private void AddProducts()
    {
        var photo = File.ReadAllBytes(
            "/home/damir/Documents/Github/PetShop-ProiectIP/PetShop/Integration/Persistence/DbSeeder/Resources/carte1.jpg");
        var productDto = new ProductDto
        {
            Name = "Raspundel Istetel - Biblia pentru copii. Povestiri",
            Description = "Stii care este cea mai citita carte din lume, numita si „cartea cartilor”? Este Biblia si a fost tradusa in toate limbile. Cartea noastra iti va dezvalui cele mai interesante povesti din Vechiul si Noul Testament intr-o maniera distractiva si interactiva.\n\nPrin intermediul cartii Biblia pentru copii - Povestiri vei descoperi ce spun diverse personaje biblice si vei putea asculta rugaciuni si cantece pentru copii. De asemenea, te vei bucura de ilustratii frumoase si colorate, iar cu ajutorul intrebarilor de pe fiecare pagina vei putea verifica singur ce informatii ai retinut.",
            Price = 100,
            Quantity = 1000,
            Category = "Carti smekere",
            Link = "https://cdn.dc5.ro/img-prod/3147050990-0.png"
        };
        Assert.That(PersistenceAccess.Instance.ProductRepository?.RegisterProduct(productDto).IsSuccess, Is.EqualTo(true));
        
        Console.WriteLine(PersistenceAccess.Instance.ProductRepository.GetAllProducts().SuccessValue);
    }
    
    private void AddOrders()
    {
        
    }
    
    [Test]
    public void AddEntities()
    {
        PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Integration);
        
        AddUsers();
        AddProviders();
        AddAdmins();
        AddProducts();
        AddOrders();
    }
}