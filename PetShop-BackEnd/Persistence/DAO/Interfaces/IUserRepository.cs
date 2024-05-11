using Persistence.DTO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Persistence.DAO.Interfaces;

public interface IUserRepository
{
    bool RegisterUser(UserInfoDto userInfo);
    bool UpdateUser(string username, UserInfoDto userDtoInfoDto);
    bool DeleteUser(string username);
    List<BillUserDto?> GetAllUsers();
    BillUserDto? GetUser(string username);
    string? GetUserPassword(string username);
    string? GetUserType(string username);
    BillDto? GetBillingDetails(string username);
}