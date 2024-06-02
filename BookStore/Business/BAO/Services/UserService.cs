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
                $"Could not register the user {userRegisterDto.Username}.");

        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userRegisterDto);
        var register = _persistenceFacade.UserRepository.RegisterUser(gdprUserInfoDto);

        _logger.LogInformation(register.Message);

        return !register.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while register {userRegisterDto.Username}")
            : Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {userRegisterDto.Username} successfully registered.");
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
            var userKey = PersistenceFacade.Instance.UserRepository.GetUserPassword(result.SuccessValue[i].Username);
            result.SuccessValue[i] = GdprMapper.UndoUserInfoDtoGdpr(result.SuccessValue[i], userKey.SuccessValue);
        }

        return result.SuccessValue.Count > 1
            ? Result<IList<UserInfoDto>, BaoErrorType>.Success(
                result.SuccessValue.Where(u => u.Username != requester).ToList())
            : Result<IList<UserInfoDto>, BaoErrorType>.Fail(BaoErrorType.UsersNotFound, result.Message);
    }

    public Result<UserInfoDto, BaoErrorType> GetUserInfo(string username)
    {
        var encryptionKey = AuthService.GetEncryptionKey(username);
        if (!encryptionKey.IsSuccess)
            return Result<UserInfoDto, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);

        var result = _persistenceFacade.UserRepository.GetUser(username);

        _logger.LogInformation(result.Message);

        return result.IsSuccess
            ? Result<UserInfoDto, BaoErrorType>.Success(GdprMapper.UndoUserInfoDtoGdpr(result.SuccessValue, encryptionKey.SuccessValue))
            : Result<UserInfoDto, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while retrieving info for user {username}");
    }

    public Result<BillDto, BaoErrorType> GetUserBillInfo(string username)
    {
        var encryptionKey = AuthService.GetEncryptionKey(username);
        if (!encryptionKey.IsSuccess)
            return Result<BillDto, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);

        var result = _persistenceFacade.BillRepository.GetBillingDetails(username);

        _logger.LogInformation(result.Message);

        return result.IsSuccess
            ? Result<BillDto, BaoErrorType>.Success(GdprMapper.UndoBillGdpr(result.SuccessValue, encryptionKey.SuccessValue))
            : Result<BillDto, BaoErrorType>.Fail(BaoErrorType.BillDetailsNotFounded,
                $"Error while retrieving bill info for user {username}");
    }

    public Result<VoidResult, BaoErrorType> UpdateUser(string username, UserRegisterDto userRegisterDto)
    {
        var encryptionKey = AuthService.GetEncryptionKey(username);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);

        var gdprUserInfoDto = GdprMapper.DoUserInfoDtoGdpr(userRegisterDto);

        var result = _persistenceFacade.UserRepository.UpdateUser(username, gdprUserInfoDto);
        _logger.LogInformation(result.Message);

        if (!result.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToUpdate,
                $"Database error while updating user {username}");

        if (userRegisterDto.Password == "")
            return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully updated.");

        var billDetails = _persistenceFacade.BillRepository.GetBillingDetails(gdprUserInfoDto.Username);
        if (!billDetails.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.FailedToEncryptBillDetails,
                $"Bill details of {username}, not found.");

        var decryptedDetails = GdprMapper.UndoBillGdpr(billDetails.SuccessValue, encryptionKey.SuccessValue);
        _persistenceFacade.BillRepository.UpdateBillByUsername(gdprUserInfoDto.Username,
            GdprMapper.DoBillGdpr(decryptedDetails, gdprUserInfoDto.Password));

        return Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
            $"User {username} successfully updated and encrypted.");
    }

    public Result<VoidResult, BaoErrorType> UpdateBill(string username, BillDto billDto)
    {
        var encryptionKey = AuthService.GetEncryptionKey(username);
        if (!encryptionKey.IsSuccess)
            return Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.KeyNotFound, encryptionKey.Message);
        
        var result = _persistenceFacade.BillRepository.UpdateBillByUsername(username, GdprMapper.DoBillGdpr(billDto, encryptionKey.SuccessValue));

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

        var result = _persistenceFacade.UserRepository.DeleteUser(username);

        _logger.LogInformation(result.Message);

        return result.IsSuccess
            ? Result<VoidResult, BaoErrorType>.Success(VoidResult.Get(),
                $"User {username} successfully deleted.")
            : Result<VoidResult, BaoErrorType>.Fail(BaoErrorType.DatabaseError,
                $"Database error while deleting user {username}");
    }
}