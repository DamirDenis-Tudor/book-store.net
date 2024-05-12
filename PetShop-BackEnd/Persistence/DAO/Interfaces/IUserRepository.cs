using Logger;
using Persistence.DTO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Persistence.DAO.Interfaces;

public interface IUserRepository
{
    Result<bool, DaoErrorType> RegisterUser(UserInfoDto userInfo);
    Result<bool, DaoErrorType> UpdateUser(string username, UserInfoDto userDtoInfoDto);
    Result<bool, DaoErrorType> DeleteUser(string username);
    Result<List<BillUserDto>, DaoErrorType> GetAllUsers();
    Result<BillUserDto, DaoErrorType> GetUser(string username);
    Result<string, DaoErrorType> GetUserPassword(string username);
    Result<string, DaoErrorType> GetUserType(string username);
    Result<BillDto, DaoErrorType> GetBillingDetails(string username);
}