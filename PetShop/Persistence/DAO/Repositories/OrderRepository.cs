/**************************************************************************
 *                                                                        *
 *  Description: OrderRepository                                          *
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

using System.Data.Common;
using Logger;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.DTO.Order;

namespace Persistence.DAO.Repositories;

internal class OrderRepository(PersistenceAccess.DatabaseContext dbContext) : IOrderRepository
{
    public Result<bool, DaoErrorType> RegisterOrderSession(OrderSessionDto orderSessionDto)
    {
        try
        {
            var existingOrderSession = dbContext.OrdersSessions.FirstOrDefault(o =>
                o.SessionCode == orderSessionDto.SessionCode
            );
            if (existingOrderSession != null)
                return Result<bool, DaoErrorType>.Fail(
                    DaoErrorType.AlreadyRegistered,
                    $"Order with sessionCode {orderSessionDto.SessionCode} is already present."
                );
            var orderSession = MapperDto.MapToOrderSession(orderSessionDto);
            orderSession.User = dbContext.Users.FirstOrDefault(u => u.Username == orderSessionDto.Username);
            orderSessionDto.OrderProducts.ToList().ForEach(op =>
                {
                    var orderProduct = MapperDto.MapToOrderProduct(op);
                    
                    orderProduct.Product = dbContext.Products.Include(p => p.OrderProducts)
                        .FirstOrDefault(p =>
                        p.Name == op.ProductName);
                    
                    orderSession.TotalPrice += orderProduct.Product!.Price * orderProduct.Quantity;
                    
                    orderSession.OrderProducts.Add(orderProduct);
                }
            );
            
            dbContext.Add(orderSession);
            dbContext.SaveChanges();
        }
        catch (DbException e)
        {
            return Result<bool, DaoErrorType>.Fail(
                DaoErrorType.DatabaseError,
                $"Order with sessionCode {orderSessionDto.SessionCode} failed to add."
            );
        }

        return Result<bool, DaoErrorType>.Success(
            true,
            $"Order with sessionCode {orderSessionDto.SessionCode} wad added."
        );
    }

    public Result<bool, DaoErrorType> DeleteOrderSession(string sessionCode)
    {
        try
        {
            var existingOrderSession = dbContext.OrdersSessions.FirstOrDefault(o => o.SessionCode == sessionCode);

            if (existingOrderSession == null)
                return Result<bool, DaoErrorType>.Fail(
                    DaoErrorType.NotFound,
                    $"OrderSession {sessionCode} could not be deleted."
                );

            dbContext.OrdersSessions.Remove(existingOrderSession);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<bool, DaoErrorType>.Fail(
                DaoErrorType.DatabaseError,
                $"OrderSession {sessionCode} could not be deleted."
            );
        }

        return Result<bool, DaoErrorType>.Success(true, $"OrderSession added successfully.");
    }

    public Result<OrderSessionDto, DaoErrorType> GetSessionOrder(string sessionCode)
    {
        var orderSessionDto = MapperDto.MapToOrderSessionDto(
            dbContext.OrdersSessions
                .Include(user => user.OrderProducts)
                .FirstOrDefault(o => o.SessionCode == sessionCode)
        );

        return orderSessionDto == null
            ? Result<OrderSessionDto, DaoErrorType>.Fail(DaoErrorType.NotFound,
                $"OrderSession {sessionCode} not found.")
            : Result<OrderSessionDto, DaoErrorType>.Success(orderSessionDto, $"OrderSession {sessionCode} not found");
    }

    public Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrdersByUsername(string username)
    {
        var orderSessions = dbContext.OrdersSessions
            .Include(o => o.User)
            .Where(o => o.User != null && o.User.Username == username)
            .ToList();

        var orderSessionDtos = orderSessions.Select(MapperDto.MapToOrderSessionDto).ToList();

        return orderSessions.Count!=0 && orderSessionDtos.Count != 0
            ? Result<IList<OrderSessionDto>, DaoErrorType>.Success(orderSessionDtos!, "OrderSessions list returned.")
            : Result<IList<OrderSessionDto>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "No order session found.");
    }


    public Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrders()
    {
        var orderSessions = new List<OrderSessionDto>();
        dbContext.OrdersSessions.ToList().ForEach(o => orderSessions.Add(MapperDto.MapToOrderSessionDto(o)!));
        return orderSessions.Count != 0
            ? Result<IList<OrderSessionDto>, DaoErrorType>
                .Success(orderSessions, "OrderSessions list returned.")
            : Result<IList<OrderSessionDto>, DaoErrorType>
                .Fail(DaoErrorType.ListIsEmpty, "No order session found.");
    }
}