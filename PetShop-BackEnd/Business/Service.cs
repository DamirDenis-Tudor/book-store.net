using Persistence.DAL;
using Persistence.DTO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;
using Persistence.Entity;

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

        var deliveryRepository = new PersistenceAccess().BillRepository;
        var billDto = new BillDto
        {
            Address = "Stefan cel mare",
            Telephone = "0759123443",
            Country = "Franta",
            City = "Iasi",
            PostalCode = "707071"
        };
        //userRepo.DeleteUser("damir_12345");
        //deliveryRepository.UpdateBillToUsername("damir_12345", billDto);
        deliveryRepository.UpdateBillToUsername("damir_12345", new BillDto{});

        return userRepo.GetUser("damir_12345");
    }
}