using System.Runtime.CompilerServices;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.DTO.Bill;
using Persistence.Entity;

namespace Persistence.DAO.Repositories;

internal class BillRepository(PersistenceAccess.DatabaseContext dbContext) : IBillRepository
{
    private readonly ILogger _logger = Logging.Instance.GetLogger<BillRepository>();

    public bool UpdateBillToUsername(string username, BillDto billDto)
    {
        try
        {
            var existingUser = dbContext.Users.Include(u => u.BillDetails)
                .FirstOrDefault(u => u.Username == username);

            if (existingUser == null) return false;
            if (existingUser.BillDetails.Address != "") existingUser.BillDetails.Address = billDto.Address;
            if (existingUser.BillDetails.Country != "") existingUser.BillDetails.Country = billDto.Country;
            if (existingUser.BillDetails.PostalCode != "") existingUser.BillDetails.PostalCode = billDto.PostalCode;
            if (existingUser.BillDetails.Telephone != "") existingUser.BillDetails.Telephone = billDto.Telephone;
            if (existingUser.BillDetails.City != "") existingUser.BillDetails.City = billDto.City;

            dbContext.Users.Update(existingUser);
            dbContext.SaveChanges();

            return true;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError("Could not add bill to user: {ErrorMessage}", e.Message);
            return false;
        }
    }
}