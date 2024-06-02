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
using Common;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;using Persistence.DTO.Order;
using Persistence.Mappers;

namespace Persistence.DAO.Repositories;

internal class OrderRepository(DatabaseContext dbContext) : IOrderRepository
{
    public Result<VoidResult, DaoErrorType> RegisterOrderSession(OrderSessionDto orderSessionDto)
    {
        try
        {
            var existingOrderSession = dbContext.OrdersSessions.FirstOrDefault(o =>
                o.SessionCode == orderSessionDto.SessionCode
            );
            if (existingOrderSession != null)
                return Result<VoidResult, DaoErrorType>.Fail(
                    DaoErrorType.AlreadyRegistered,
                    $"Order with sessionCode {orderSessionDto.SessionCode} is already present."
                );
            var orderSession = MapperDto.MapToOrderSession(orderSessionDto);
            orderSession.User = dbContext.Users.FirstOrDefault(u => u.Username == orderSessionDto.Username);
            
            orderSessionDto.OrderProducts.ToList().ForEach(op =>
                {
                    var orderProduct = MapperDto.MapToOrderProduct(op);
                    
                    orderProduct.Product = dbContext.Products
                        .Include(p => p.OrderProducts)
                        .Include(p => p.ProductInfo)
                        .FirstOrDefault(p =>
                            p.ProductInfo.Name == op.ProductInfoDto.Name);

                    orderProduct.ProductInfo = dbContext.ProductInfos
                        .FirstOrDefault(p => p.Name == op.ProductInfoDto.Name)!;
                    
                    orderSession.OrderProducts.Add(orderProduct);
                }
            );

            dbContext.Add(orderSession);
            dbContext.SaveChanges();
        }
        catch (DbException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(
                DaoErrorType.DatabaseError,
                $"Order with sessionCode {orderSessionDto.SessionCode} failed to add: {e.Message}"
            );
        }

        return Result<VoidResult, DaoErrorType>.Success(
            VoidResult.Get(), $"Order with sessionCode {orderSessionDto.SessionCode} wad added."
        );
    }

    public Result<VoidResult, DaoErrorType> DeleteOrderSession(string sessionCode)
    {
        try
        {
            var existingOrderSession = dbContext.OrdersSessions.FirstOrDefault(o => o.SessionCode == sessionCode);

            if (existingOrderSession == null)
                return Result<VoidResult, DaoErrorType>.Fail(
                    DaoErrorType.NotFound, $"OrderSession {sessionCode} could not be deleted."
                );

            dbContext.OrdersSessions.Remove(existingOrderSession);
            dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            return Result<VoidResult, DaoErrorType>.Fail(
                DaoErrorType.DatabaseError,
                $"OrderSession {sessionCode} could not be deleted:  {e.Message}"
            );
        }

        return Result<VoidResult, DaoErrorType>.Success(VoidResult.Get(), $"OrderSession added successfully.");
    }

    public Result<OrderSessionDto, DaoErrorType> GetSessionOrder(string sessionCode)
    {
        var orderSessionDto = MapperDto.MapToOrderSessionDto(
            dbContext.OrdersSessions
                .Include(orderSession => orderSession.OrderProducts)
                .Include(orderSession => orderSession.User)
                .FirstOrDefault(o => o.SessionCode == sessionCode)
        );

        return orderSessionDto == null
            ? Result<OrderSessionDto, DaoErrorType>.Fail(DaoErrorType.NotFound,
                $"OrderSession {sessionCode} not found.")
            : Result<OrderSessionDto, DaoErrorType>.Success(orderSessionDto, $"OrderSession {sessionCode} found");
    }

    public Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrdersByUsername(string username)
    {
        var orderSessions = dbContext.OrdersSessions
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .Where(o => o.User != null && o.User.Username == username)
            .ToList();

        var orderSessionDtos = orderSessions.Select(orderSession =>
        {
            dbContext.Entry(orderSession).Reload();
            return MapperDto.MapToOrderSessionDto(orderSession);
        }).ToList();
        
        orderSessionDtos.Reverse();
        
        return orderSessions.Count != 0 && orderSessionDtos.Count != 0
            ? Result<IList<OrderSessionDto>, DaoErrorType>.Success(orderSessionDtos!, "OrderSessions list returned.")
            : Result<IList<OrderSessionDto>, DaoErrorType>.Fail(DaoErrorType.ListIsEmpty, "No order session found.");
    }


    public Result<IList<OrderSessionDto>, DaoErrorType> GetAllOrders()
    {
        var orderSessions = new List<OrderSessionDto>();
        dbContext.OrdersSessions
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .ToList()
            .ForEach(o => orderSessions.Add(MapperDto.MapToOrderSessionDto(o)!));
        
        return orderSessions.Count != 0
            ? Result<IList<OrderSessionDto>, DaoErrorType>
                .Success(orderSessions, "OrderSessions list returned.")
            : Result<IList<OrderSessionDto>, DaoErrorType>
                .Fail(DaoErrorType.ListIsEmpty, "No order session found.");
    }
}