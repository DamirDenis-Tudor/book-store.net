using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Repositories;

internal class UserRepository(PersistenceAccess.DatabaseContext dbContext) : IUserRepository
{
    public bool RegisterUser(UserDto user)
    {
        /*try
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }*/

        return true;
    }

    public List<UserDto> GetAllUsers()
    {
        var users= dbContext.Users.ToList();
        return new List<UserDto>();
    }

    public UserDto? GetUser(string username)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Username == username);
        return new UserDto();
    }

    public string? GetUserPassword(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.Password;


    public string? GetUserType(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.UserType;

    public BillDto? GetBillingDetails(string username)
    {
        var billDetails = dbContext.Users.Include(user => user.BillDetails)
            .FirstOrDefault(u => u.Username == username)?.BillDetails;
        return new BillDto();
    }

}