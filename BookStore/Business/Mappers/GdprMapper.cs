using Business.BTO;
using Business.Utilities;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.User;

namespace Business.Mappers;

public static class GdprMapper
{
    public static UserInfoDto DoUserInfoDtoGdpr(UserInfoDto userInfoDto)
    {
        return new UserInfoDto
        {
            FirstName = GdprUtility.Encrypt(userInfoDto.FirstName),
            LastName = GdprUtility.Encrypt(userInfoDto.LastName),
            Username = GdprUtility.Encrypt(userInfoDto.Username),
            Password = GdprUtility.Hash(userInfoDto.Password),
            Email = GdprUtility.Encrypt(userInfoDto.Email),
            UserType = GdprUtility.Encrypt(userInfoDto.UserType)
        };
    }
    
    public static BillUserDto DoBillUserDtoGdpr(BillUserDto billUserDto)
    {
        return new BillUserDto
        {
            FirstName = GdprUtility.Encrypt(billUserDto.FirstName),
            LastName = GdprUtility.Encrypt(billUserDto.LastName),
            Username = GdprUtility.Encrypt(billUserDto.Username),
            Email = GdprUtility.Encrypt(billUserDto.Email)
        };
    }

    public static BillUserDto UndoBillUserDtoGdpr(BillUserDto encryptedBillUserDto)
    {
        return new BillUserDto
        {
            FirstName = GdprUtility.Decrypt(encryptedBillUserDto.FirstName),
            LastName = GdprUtility.Decrypt(encryptedBillUserDto.LastName),
            Username = GdprUtility.Decrypt(encryptedBillUserDto.Username),
            Email = GdprUtility.Decrypt(encryptedBillUserDto.Email),
        };
    }
    
    public static BillDto DoBillGdpr(BillDto billDto)
    {
        return new BillDto
        {
            Address = GdprUtility.Encrypt(billDto.Address),
            Telephone = GdprUtility.Encrypt(billDto.Telephone),
            Country = GdprUtility.Encrypt(billDto.Country),
            City = GdprUtility.Encrypt(billDto.City),
            PostalCode = GdprUtility.Encrypt(billDto.PostalCode)
        };
    }

    public static BillDto UndoBillGdpr(BillDto encryptedBillDto)
    {
        return new BillDto
        {
            Address = GdprUtility.Decrypt(encryptedBillDto.Address),
            Telephone = GdprUtility.Decrypt(encryptedBillDto.Telephone),
            Country = GdprUtility.Decrypt(encryptedBillDto.Country),
            City = GdprUtility.Decrypt(encryptedBillDto.City),
            PostalCode = GdprUtility.Decrypt(encryptedBillDto.PostalCode)
        };
    }

    public static OrderSessionDto DoOrderSessionDtoGdpr(OrderSessionDto orderSessionDto)
    {
        orderSessionDto.Username = GdprUtility.Encrypt(orderSessionDto.Username);
        return orderSessionDto;
    }

    public static OrderSessionDto UndoOrderSessionDtoGdpr(OrderSessionDto encryptedOrderSessionDto)
    {
        encryptedOrderSessionDto.Username = GdprUtility.Decrypt(encryptedOrderSessionDto.Username);
        return encryptedOrderSessionDto;
    }

    public static OrderBto DoOrderBto(OrderBto orderBto)
    {
        orderBto.Username = GdprUtility.Encrypt(orderBto.Username);
        return orderBto;
    }
    
    public static UserLoginBto DoUserLoginBto(UserLoginBto userLoginBto)
    {
        return new UserLoginBto
        {
            Username = GdprUtility.Encrypt(userLoginBto.Username),
            Password = GdprUtility.Hash(userLoginBto.Password),
        };
    }
}