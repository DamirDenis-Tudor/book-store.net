using Persistence.DTO;

namespace Persistence.DAO.Interfaces;

public interface IUserRepository
{
    bool RegisterUser(UserInfoDto userInfo);
    List<BillUserDto?> GetAllUsers();
    BillUserDto? GetUser(string username);
    string? GetUserPassword(string username);
    string? GetUserType(string username);
    BillDto? GetBillingDetails(string username);
}