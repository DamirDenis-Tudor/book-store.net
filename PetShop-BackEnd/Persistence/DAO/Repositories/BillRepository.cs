using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO.Bill;

namespace Persistence.DAO.Repositories;

internal class BillRepository(PersistenceAccess.DatabaseContext dbContext) : IBillRepository
{
    public Result<string, DaoErrorType> UpdateBillToUsername(string username, BillDto billDto)
    {
        try
        {
            var billDetails = dbContext.Bills.Include(b => b.User)
                .FirstOrDefault(b => b.User != null && b.User.Username == username);

            if (billDetails == null)
                return Result<string, DaoErrorType>.Fail(DaoErrorType.CouldNotUpdate, "Bill not found.");

            if (!string.IsNullOrEmpty(billDto.Address)) billDetails.Address = billDto.Address;
            if (!string.IsNullOrEmpty(billDto.Country)) billDetails.Country = billDto.Country;
            if (!string.IsNullOrEmpty(billDto.PostalCode)) billDetails.PostalCode = billDto.PostalCode;
            if (!string.IsNullOrEmpty(billDto.Telephone)) billDetails.Telephone = billDto.Telephone;
            if (!string.IsNullOrEmpty(billDto.City)) billDetails.City = billDto.City;

            dbContext.Entry(billDetails.User!).State = EntityState.Detached;
            dbContext.Update(billDetails);
            dbContext.SaveChanges();

            return Result<string, DaoErrorType>.Success("Bill updated successfully.");
        }
        catch (DbUpdateException e)
        {
            return Result<string, DaoErrorType>.Fail(DaoErrorType.CouldNotUpdate,
                "Database error occurred while updating bill.");
        }
    }
}