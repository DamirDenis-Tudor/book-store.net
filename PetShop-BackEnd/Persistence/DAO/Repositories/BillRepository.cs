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
            var billDetails = dbContext.Bills.Include(u => u.User)
                .FirstOrDefault(b => b.User != null && b.User.Username== username);

            if (billDetails == null)
            {
                _logger.LogWarning("Bill not found.");
                return false;
            }
            if (billDto.Address != "") billDetails.Address = billDto.Address;
            if (billDto.Country != "") billDetails.Country = billDto.Country;
            if (billDto.PostalCode != "") billDetails.PostalCode = billDto.PostalCode;
            if (billDto.Telephone != "") billDetails.Telephone = billDto.Telephone;
            if (billDto.City != "") billDetails.City = billDto.City;

            dbContext.Update(billDetails);
            dbContext.SaveChanges();

            return true;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError("Could not add bill to user: {ErrorMessage}", e);
            return false;
        }
    }
}