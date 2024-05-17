/**************************************************************************
 *                                                                        *
 *  Description: DTO's - ENTITIES MAPPER                                  *
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


using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Persistence.DTO.User;
using Persistence.Entity;

namespace Persistence.DTO;

/// <summary>
/// Class for mapping between DTOs and Entities.
/// </summary>
internal class MapperDto
{
    /// <summary>
    /// Maps a UserInfoDto to a User entity.
    /// </summary>
    /// <param name="userInfoDto">The UserInfoDto to map from.</param>
    /// <returns>A User entity.</returns>
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

    /// <summary>
    /// Maps a User entity to a UserInfoDto.
    /// </summary>
    /// <param name="user">The User entity to map from.</param>
    /// <returns>A UserInfoDto or null if the user is null.</returns>
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

    /// <summary>
    /// Maps a User entity to a BillUserDto.
    /// </summary>
    /// <param name="user">The User entity to map from.</param>
    /// <returns>A BillUserDto or null if the user is null.</returns>
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

    /// <summary>
    /// Maps a BillDetails entity to a BillDto.
    /// </summary>
    /// <param name="bill">The BillDetails entity to map from.</param>
    /// <returns>A BillDto or null if the bill is null.</returns>
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

    /// <summary>
    /// Maps a ProductDto to a Product entity.
    /// </summary>
    /// <param name="productDto">The ProductDto to map from.</param>
    /// <returns>A Product entity.</returns>
    internal static Entity.Product MapToProduct(ProductDto productDto)
    {
        return new Entity.Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Quantity = productDto.Quantity,
            OrderProducts = new List<OrderProduct>(),
            Category = productDto.Category
        };
    }

    /// <summary>
    /// Maps a Product entity to a ProductDto.
    /// </summary>
    /// <param name="product">The Product entity to map from.</param>
    /// <returns>A ProductDto or null if the product is null.</returns>
    internal static ProductDto? MapToProductDto(Entity.Product? product)
    {
        if (product == null) return null;
        return new ProductDto
        {
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Photo = product.Photo,
            Category = product.Category
        };
    }

    /// <summary>
    /// Maps an OrderProduct entity to an OrderProductDto.
    /// </summary>
    /// <param name="orderProduct">The OrderProduct entity to map from.</param>
    /// <returns>An OrderProductDto or null if the orderProduct is null.</returns>
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

    /// <summary>
    /// Maps an OrderSession entity to an OrderSessionDto.
    /// </summary>
    /// <param name="orderSession">The OrderSession entity to map from.</param>
    /// <returns>An OrderSessionDto or null if the orderSession is null.</returns>
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

    /// <summary>
    /// Maps an OrderProductDto to an OrderProduct entity.
    /// </summary>
    /// <param name="orderProductDto">The OrderProductDto to map from.</param>
    /// <returns>An OrderProduct entity.</returns>
    internal static OrderProduct MapToOrderProduct(OrderProductDto orderProductDto)
    {
        return new OrderProduct
        {
            OrderProductName = orderProductDto.ProductName,
            Quantity = orderProductDto.Quantity,
        };
    }

    /// <summary>
    /// Maps an OrderSessionDto to an OrderSession entity.
    /// </summary>
    /// <param name="orderSessionDto">The OrderSessionDto to map from.</param>
    /// <returns>An OrderSession entity.</returns>
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
