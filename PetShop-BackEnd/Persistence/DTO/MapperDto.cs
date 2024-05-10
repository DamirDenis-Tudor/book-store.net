using Persistence.Entity;

namespace Persistence.DTO;

internal class MapperDto
{
    internal static User MapToUser(UserInfoDto userInfoDto) =>
        new()
        {
            FirstName = userInfoDto.FirstName,
            LastName = userInfoDto.LastName,
            Username = userInfoDto.Username,
            Password = userInfoDto.Password,
            Email = userInfoDto.Email,
            UserType = userInfoDto.UserType
        };

    internal static UserInfoDto? MapToUserDto(User? user)
    {
        return user != null
            ? new UserInfoDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                UserType = user.UserType
            }
            : null;
    }

    internal static BillUserDto? MapToBillUserDto(User? user)
    {
        if (user == null) return null;

        return new BillUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            BillDto = MapToBillDto(user?.BillDetails)
        };
    }

    internal static BillDto? MapToBillDto(BillDetails? bill)
    {
        if (bill == null) return null;

        return new BillDto
        {
            Address = bill.Address,
            Telephone = bill.Telephone,
            Country = bill.Country,
            City = bill.City,
            PostalCode = bill.PostalCode
        };
    }

    internal static BillDetails MapToBill(BillDto bill)
    {
        return new BillDetails
        {
            Address = bill.Address,
            Telephone = bill.Telephone,
            Country = bill.Country,
            City = bill.City,
            PostalCode = bill.PostalCode
        };
    }
    
}