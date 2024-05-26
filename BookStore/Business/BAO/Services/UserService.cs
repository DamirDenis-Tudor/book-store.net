using Business.BAO.Interfaces;
using Business.Mappers;
using Business.Utilities;
using Common;
using Persistence.DAL;
using Persistence.DAO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Business.BAO.Services;

public class UserService : IUsers
{
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;

    public Result<VoidResult, BaoErrorType> RegisterUser(UserInfoDto userInfoDto)
    {
        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userInfoDto);

        var result = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);

        if (result.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {userInfoDto.Username} succesfully registered.");

        return result.ErrorType == DaoErrorType.AlreadyRegistered
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidRegisterData,
                $"Invalid register data {userInfoDto.Username}")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while register {userInfoDto.Username}");
    }

    public Result<VoidResult, BaoErrorType> RegisterProvider(string requester, UserInfoDto userInfoDto)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        if (userInfoDto.UserType != "PROVIDER")
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidUserType,
                $"Could not register the user {userInfoDto.Username}. Usertype not PROVIDER.");

        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userInfoDto);
        var register = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);
        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while register {userInfoDto.Username}")
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {userInfoDto.Username} succesfully registered.");
    }

    public Result<IList<BillUserDto>, BaoErrorType> GetAllUsers(string requester)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<IList<BillUserDto>, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        var result = _persistenceFacade.UserRepository.GetAllUsers();
        return result.IsSuccess
            ? Result<IList<BillUserDto>, BaoErrorType>.Success(result.SuccessValue)
            : Result<IList<BillUserDto>, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                "Database error while retrieving all users.");
    }

    public Result<BillUserDto, BaoErrorType> GetUserInfo(string username)
    {
        var result = _persistenceFacade.UserRepository.GetUser(username);

        return result.IsSuccess
            ? Result<BillUserDto, BaoErrorType>.Success(result.SuccessValue)
            : Result<BillUserDto, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while retrieving info for user {username}");
    }

    public Result<BillDto, BaoErrorType> GetUserBillInfo(string username)
    {
        var result = _persistenceFacade.BillRepository.GetBillingDetails(username);

        return result.IsSuccess
            ? Result<BillDto, BaoErrorType>.Success(result.SuccessValue)
            : Result<BillDto, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while retrieving bill info for user {username}");
    }

    public Result<VoidResult, BaoErrorType> UpdateUser(string username, UserInfoDto userInfoDto)
    {
        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userInfoDto);

        var result = _persistenceFacade.UserRepository.UpdateUser(username, gdprUserInfoDto);
        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully updated.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while updating user {username}");
    }

    public Result<VoidResult, BaoErrorType> UpdateBill(string username, BillDto billDto)
    {
        var result = _persistenceFacade.BillRepository.UpdateBillByUsername(username, billDto);

        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"Bill for user {username} successfully updated.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while updating bill for user {username}");
    }

    public Result<VoidResult, BaoErrorType> DeleteUser(string requester, string username)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        var result = _persistenceFacade.UserRepository.DeleteUser(username);
        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully deleted.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while deleting user {username}");
    }
}