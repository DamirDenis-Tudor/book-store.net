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
    private readonly ILogger _logger = Logger.Logger.Instance.GetLogger<UserRepository>();

    public Result<bool, DaoErrorType> RegisterUser(UserInfoDto userDtoInfoDto)
    {
        try
        {
            if (GetUser(userDtoInfoDto.Username).IsSuccess)
                throw new DbUpdateException();
            
            var user = MapperDto.MapToUser(userDtoInfoDto);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<bool, DaoErrorType>.Fail(
                DaoErrorType.AlreadyRegistered,
                $"User {userDtoInfoDto.Username} already exist. Caused by {nameof(DbUpdateException)}."
            );
        }

        return Result<bool, DaoErrorType>.Success(true,$"User {userDtoInfoDto.Username} registered.");
    }

    public Result<bool, DaoErrorType> UpdateUser(string username, UserInfoDto userDtoInfoDto)
    {
        try
        {
            var existingUser = dbContext.Users.Include(user => user.BillDetails)
                .FirstOrDefault(user => user.Username == username);

            if (existingUser == null)
            {
                return Result<bool, DaoErrorType>.Fail(
                    DaoErrorType.NotFound,
                    $"User {userDtoInfoDto.Username} not found. Caused by existingUser={existingUser}."
                );
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
            return Result<bool, DaoErrorType>.Fail(
                DaoErrorType.AlreadyRegistered,
                $"User {userDtoInfoDto.Username} cannot be updated. Caused by {nameof(DbUpdateException)}."
            );
        }

        return Result<bool, DaoErrorType>.Success(true,$"User {username} updated successfully: {userDtoInfoDto}");
    }

    public Result<bool, DaoErrorType> DeleteUser(string username)
    {
        try
        {
            var existingUser = dbContext.Users.Include(user => user.BillDetails)
                .FirstOrDefault(user => user.Username == username);

            if (existingUser == null)
            {
                return Result<bool, DaoErrorType>.Fail(
                    DaoErrorType.NotFound,
                    $"User {username} could not be deleted. Caused by existingUser={existingUser}."
                );
            }

            using var dbContextTransaction = dbContext.Database.BeginTransaction();
            dbContext.Users.Remove(existingUser);
            dbContext.SaveChanges();
            dbContextTransaction.Commit();
        }
        catch (DbUpdateException e)
        {
            return Result<bool, DaoErrorType>.Fail(
                DaoErrorType.DatabaseError,
                $"User {username} cannot be updated. Caused by {nameof(DbUpdateException)}."
            );
        }

        return Result<bool, DaoErrorType>.Success(true,$"User {username} updated successfully.");
    }

    public Result<List<BillUserDto>, DaoErrorType> GetAllUsers()
    {
        var users = new List<BillUserDto>();
        dbContext.Users.ToList().ForEach(u => users.Add(MapperDto.MapToBillUserDto(u)!));

        return users.Count != 0
            ? Result<List<BillUserDto>, DaoErrorType>.Success(users, "Users list returned.")
            : Result<List<BillUserDto>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "No user found.");
    }

    public Result<BillUserDto, DaoErrorType> GetUser(string username)
    {
        var billUserDto = MapperDto.MapToBillUserDto(
            dbContext.Users
                .Include(user => user.BillDetails)
                .FirstOrDefault(u => u.Username == username)
        );

        return billUserDto == null
            ? Result<BillUserDto, DaoErrorType>.Fail(DaoErrorType.NotFound, $"User {username} not found")
            : Result<BillUserDto, DaoErrorType>.Success(billUserDto,$"User {username} found");
    }


    public Result<string, DaoErrorType> GetUserPassword(string username)
    {
        var password = dbContext.Users.FirstOrDefault(u => u.Username == username)?.Password;
        return password == null
            ? Result<string, DaoErrorType>.Fail(DaoErrorType.NotFound,
                $"Password for user {username} not found")
            : Result<string, DaoErrorType>.Success(password, $"User {username} password found");
    }

    public Result<string, DaoErrorType> GetUserType(string username)
    {
        var userType = dbContext.Users.FirstOrDefault(u => u.Username == username)?.UserType;
        return userType == null
            ? Result<string, DaoErrorType>.Fail(DaoErrorType.NotFound,
                $"User type for user {username} not found")
            : Result<string, DaoErrorType>.Success(userType, $"UserType for {username} returned.");
    }
}