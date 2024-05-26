/**************************************************************************
 *                                                                        *
 *  Description: Logger Utility                                           *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using Business.BAO.Interfaces;
using Business.Mappers;
using Business.Utilities;
using Common;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO;
using Persistence.DTO.Bill;
using Persistence.DTO.User;

namespace Business.BAO.Services;

internal class UserService : IUsers
{
    private readonly ILogger _logger = Logger.Instance.GetLogger<UserService>();
    
    private readonly PersistenceFacade _persistenceFacade = PersistenceFacade.Instance;

    public Result<VoidResult, BaoErrorType> RegisterClient(UserRegisterDto userRegisterDto)
    {
        if (userRegisterDto.UserType != "CLIENT")
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidUserType,
                $"Username {userRegisterDto.Username} could not be registered because UserType is not CLIENT.");

        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userRegisterDto);

        var result = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);

        _logger.LogInformation(result.Message);
        
        if (result.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {userRegisterDto.Username} succesfully registered.");

        return result.ErrorType == DaoErrorType.AlreadyRegistered
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidRegisterData,
                $"Invalid register data {userRegisterDto.Username}")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while register {userRegisterDto.Username}");
    }

    public Result<VoidResult, BaoErrorType> RegisterProvider(string requester, UserRegisterDto userRegisterDto)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        if (userRegisterDto.UserType != "PROVIDER")
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.InvalidUserType,
                $"Could not register the user {userRegisterDto.Username}. Usertype not PROVIDER.");

        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userRegisterDto);
        var register = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);
        
        _logger.LogInformation(register.Message);
        
        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while register {userRegisterDto.Username}")
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {userRegisterDto.Username} succesfully registered.");
    }

    public Result<IList<UserInfoDto>, BaoErrorType> GetAllUsers(string requester)
    {
        if (!UserTypeChecker.CheckIfAdmin(username: requester))
            return Result<IList<UserInfoDto>, BaoErrorType>.Fail(BaoErrorType.UserNotAllowed,
                $"Username {requester} is not ADMIN.");

        var result = _persistenceFacade.UserRepository.GetAllUsers();
        
        _logger.LogInformation(result.Message);
        
        if (!result.IsSuccess)
            return Result<IList<UserInfoDto>, BaoErrorType>.Fail(BaoErrorType.DatabaseError, result.Message);

        for (var i = 0; i < result.SuccessValue.Count; i++)
        {
            result.SuccessValue[i] = GdprMapper.UndoBillUserDtoGdpr(result.SuccessValue[i]);
        }

        return result.SuccessValue.Count > 1
            ? Result<IList<UserInfoDto>, BaoErrorType>.Success(
                result.SuccessValue.Where(u => u.Username != requester).ToList())
            : Result<IList<UserInfoDto>, BaoErrorType>.Fail(BaoErrorType.UsersNotFound, result.Message);
    }

    public Result<UserInfoDto, BaoErrorType> GetUserInfo(string username)
    {
        var result = _persistenceFacade.UserRepository.GetUser(GdprUtility.Encrypt(username));

        _logger.LogInformation(result.Message);
        
        return result.IsSuccess
            ? Result<UserInfoDto, BaoErrorType>.Success(GdprMapper.UndoBillUserDtoGdpr(result.SuccessValue))
            : Result<UserInfoDto, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while retrieving info for user {username}");
    }

    public Result<BillDto, BaoErrorType> GetUserBillInfo(string username)
    {
        var result = _persistenceFacade.BillRepository.GetBillingDetails(GdprUtility.Encrypt(username));

        _logger.LogInformation(result.Message);
        
        return result.IsSuccess
            ? Result<BillDto, BaoErrorType>.Success(GdprMapper.UndoBillGdpr(result.SuccessValue))
            : Result<BillDto, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while retrieving bill info for user {username}");
    }

    public Result<VoidResult, BaoErrorType> UpdateUser(string username, UserRegisterDto userRegisterDto)
    {
        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userRegisterDto);

        var result = _persistenceFacade.UserRepository.UpdateUser(GdprUtility.Encrypt(username), gdprUserInfoDto);
        
        _logger.LogInformation(result.Message);
        
        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully updated.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while updating user {username}");
    }

    public Result<VoidResult, BaoErrorType> UpdateBill(string username, BillDto billDto)
    {
        var result = _persistenceFacade.BillRepository.UpdateBillByUsername(GdprUtility.Encrypt(username), billDto);

        _logger.LogInformation(result.Message);
        
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

        var result = _persistenceFacade.UserRepository.DeleteUser(GdprUtility.Encrypt(username));
        
        _logger.LogInformation(result.Message);
        
        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully deleted.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while deleting user {username}");
    }
}