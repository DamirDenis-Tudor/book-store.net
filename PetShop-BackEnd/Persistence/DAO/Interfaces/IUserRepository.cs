using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Interfaces;

public interface IUserRepository
{
    bool RegisterUser(UserDto user);
    List<UserDto> GetAllUsers();
    UserDto? GetUser(string username);
    string? GetUserPassword(string username);
    string? GetUserType(string username);
    BillDto? GetBillingDetails(string username);
}