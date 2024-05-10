using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;


namespace Persistence.DAO.Repositories;

internal class UserRepository(PersistenceAccess.DatabaseContext dbContext) : IUserRepository
{
    private readonly ILogger _logger = Logging.Instance.GetLogger<UserRepository>();
    public bool RegisterUser(UserInfoDto infoDtoUserInfo)
    {
        try
        {
            dbContext.Users.Add(MapperDto.MapToUser(infoDtoUserInfo));
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            _logger.LogWarning("Could not register user: {} ",infoDtoUserInfo);
            return false;
        }

        return true;
    }

    public List<BillUserDto?> GetAllUsers()
    {
        var users = new List<BillUserDto?>();
        dbContext.Users.ToList().ForEach(u => users.Add(MapperDto.MapToBillUserDto(u)));
        return users;
    }

    public BillUserDto? GetUser(string username)
    {
        var user = dbContext.Users.Include(user => user.BillDetails)
            .FirstOrDefault(u => u.Username == username);
        return MapperDto.MapToBillUserDto(user);
    }

    public string? GetUserPassword(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.Password;


    public string? GetUserType(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.UserType;

    public BillDto? GetBillingDetails(string username)
    {
        var billDetails = dbContext.Users.Include(user => user.BillDetails)
            .FirstOrDefault(u => u.Username == username)?.BillDetails;
        return MapperDto.MapToBillDto(billDetails);
    }
}