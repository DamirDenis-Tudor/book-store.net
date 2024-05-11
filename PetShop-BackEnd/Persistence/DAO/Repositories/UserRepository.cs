using System.Runtime.CompilerServices;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

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
            _logger.LogWarning("Could not register user: {} ", e);
            return false;
        }

        return true;
    }

    public bool UpdateUser(string username, UserInfoDto userDtoInfoDto)
    {
        try
        {
            var existingUser = dbContext.Users.Include(user => user.BillDetails)
                .FirstOrDefault(user => user.Username == username);

            if (existingUser == null)
            {
                _logger.LogWarning("Could not delete user: {}", username);
                return false;
            }

            if (userDtoInfoDto.Username != "") existingUser.Username = userDtoInfoDto.Username;
            if (userDtoInfoDto.Password != "") existingUser.Password = userDtoInfoDto.Password;
            if (userDtoInfoDto.Email != "") existingUser.Email = userDtoInfoDto.Email;
            if (userDtoInfoDto.FirstName != "") existingUser.FirstName = userDtoInfoDto.FirstName;
            if (userDtoInfoDto.LastName != "") existingUser.LastName = userDtoInfoDto.LastName;

            dbContext.Update(existingUser);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            _logger.LogWarning("Could not register user: {} ", e);
            return false;
        }

        return true;
    }

    public bool DeleteUser(string username)
    {
        try
        {
            var existingUser = dbContext.Users.Include(user => user.BillDetails)
                .FirstOrDefault(user => user.Username == username);

            if (existingUser == null)
            {
                _logger.LogWarning("Could not delete user: {}", username);
                return false;
            }

            dbContext.Remove(existingUser);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            _logger.LogWarning("Could not delete user: {} ", e);
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

    public BillUserDto? GetUser(string username) =>
        MapperDto.MapToBillUserDto(dbContext.Users.Include(user => user.BillDetails)
            .FirstOrDefault(u => u.Username == username));

    public string? GetUserPassword(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.Password;


    public string? GetUserType(string username) =>
        dbContext.Users.FirstOrDefault(u => u.Username == username)?.UserType;

    public BillDto? GetBillingDetails(string username) =>
        MapperDto.MapToBillDto(dbContext.Users.Include(user => user.BillDetails)
            .FirstOrDefault(u => u.Username == username)?.BillDetails);
}