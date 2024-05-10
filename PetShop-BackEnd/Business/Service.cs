using Persistence.DAL;
using Persistence.DTO;

namespace Business;

public class Service1
{
    public BillUserDto? Test()
    {
        var userRepo = new PersistenceAccess().UserRepository;

        var user = new UserInfoDto
        {
            FirstName = "Denis",
            LastName = "Damir",
            Username = "damir_12345",
            Password = "hasu_meu",
            Email = "denis@ip.com",
            UserType = "CLIENT"
        };
        if (userRepo.RegisterUser(user))
        {
            Console.WriteLine("User registered succesfully");
        }
        Console.WriteLine(userRepo.GetUserPassword("damir_12345"));
        Console.WriteLine(userRepo.GetUserType("damir_12345"));
        
        var deliveryRepository = new PersistenceAccess().BillRepository;
        var billDto = new BillDto
        {
            Address = "Stefan cel mare",
            Telephone = "0759123443",
            Country = "Romania",
            City = "Iasi",
            PostalCode = "707071"
        };
        Console.WriteLine(deliveryRepository.DeleteBillByUsername("damir_12345"));
        return userRepo.GetUser("damir_12345");
    }
}