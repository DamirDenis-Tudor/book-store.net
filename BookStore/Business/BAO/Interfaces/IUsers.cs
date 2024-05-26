using Common;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Business.BAO.Interfaces;

public interface IUsers
{
    Result<VoidResult, BaoErrorType> RegisterClient(UserInfoDto userInfoDto);
    Result<VoidResult, BaoErrorType> RegisterProvider(string requester, UserInfoDto userInfoDto);
    Result<IList<BillUserDto>, BaoErrorType> GetAllUsers(string requester);
    Result<BillUserDto, BaoErrorType> GetUserInfo(string username);
    Result<BillDto, BaoErrorType> GetUserBillInfo(string username);
    Result<VoidResult, BaoErrorType> UpdateUser(string username, UserInfoDto userInfoDto);
    
    Result<VoidResult, BaoErrorType> UpdateBill(string username, BillDto billDto);

    Result<VoidResult, BaoErrorType> DeleteUser(string requester, string username);
}