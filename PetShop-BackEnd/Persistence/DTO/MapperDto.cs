using Logger;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;
using Persistence.Entity;

namespace Persistence.DTO;

internal class MapperDto
{
    internal static Entity.User MapToUser(UserInfoDto userInfoDto) =>
        new()
        {
            FirstName = userInfoDto.FirstName,
            LastName = userInfoDto.LastName,
            Username = userInfoDto.Username,
            Password = userInfoDto.Password,
            Email = userInfoDto.Email,
            UserType = userInfoDto.UserType,
            BillDetails = new BillDetails { }
        };

    internal static UserInfoDto? MapToUserDto(Entity.User? user)
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

    internal static BillUserDto? MapToBillUserDto(Entity.User? user)
    {
        if (user == null) return null;

        return new BillUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            BillDto = MapToBillDto(user.BillDetails)
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

    internal static Entity.Product MapToProduct(ProductDto productDto)
    {
        return new Entity.Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
            OrderProducts = new List<OrderProduct>()
        };
    }

    internal static ProductDto? MapToProductDto(Entity.Product? product)
    {
        if (product == null) return null;
        return new ProductDto
        {
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Photo = product.Photo
        };
    }

    private static OrderProductDto? MapToOrderProductDto(Entity.OrderProduct? orderProduct)
    {
        if (orderProduct == null) return null;

        return new OrderProductDto
        {
            ProductName = orderProduct.Product!.Name,
            Price = orderProduct.Product.Price * orderProduct.Quantity,
            SessionCode = orderProduct.OrderSession!.SessionCode,
            Quantity = orderProduct.Quantity
        };
    }

    internal static OrderSessionDto? MapToOrderSessionDto(Entity.OrderSession? orderSession)
    {
        if (orderSession == null) return null;

        var orderProductsDto = new List<OrderProductDto>();
        orderSession.OrderProducts.ToList().ForEach(op =>
            {
                orderProductsDto.Add(MapperDto.MapToOrderProductDto(op)!);
            }
        );

        return new OrderSessionDto
        {
            Username = orderSession.User!.Username,
            SessionCode = orderSession.SessionCode,
            Status = orderSession.SessionCode,
            OrderProducts = orderProductsDto
        };
    }

    internal static OrderProduct MapToOrderProduct(OrderProductDto orderProductDto)
    {
        return new OrderProduct
        {
            OrderProductName = orderProductDto.ProductName,
            Quantity = orderProductDto.Quantity,
        };
    }

    internal static OrderSession MapToOrderSession(OrderSessionDto orderSessionDto)
    {
        return new OrderSession
        {
            SessionCode = orderSessionDto.SessionCode,
            Status = orderSessionDto.Status,
            OrderProducts = new List<OrderProduct>()
        };
    }
}