using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;

namespace Persistence.DAO.Repositories;

internal class BillRepository(PersistenceAccess.DatabaseContext dbContext) : IBillRepository
{
    private readonly ILogger _logger = Logging.Instance.GetLogger<BillRepository>();
    public bool AttachBillToUsername(string username, BillDto billDto)
    {
        try
        {
            var bill = MapperDto.MapToBill(billDto);
            dbContext.Bills.Add(bill);
            dbContext.SaveChanges();

            var user = dbContext.Users
                .Include(user => user.BillDetails)
                .FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return false;
            }

            user.BillDetails = bill;
            dbContext.SaveChanges();
            return true;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError("Could not add bill to: {Username}", username);
            return false;
        }
    }

    public bool DeleteBillByUsername(string username)
    {
        try
        {
            var bill = dbContext.Users.Include(user => user.BillDetails)
                .FirstOrDefault(u => u.Username == username)?.BillDetails;

            if (bill == null)
            {
                _logger.LogWarning("Bill not found for: {Username}", username);
                return false;
            }

            dbContext.Bills.Remove(bill);
        
            dbContext.SaveChanges();
        
            return true;
        }
        catch (Exception e)
        {
            _logger.LogWarning("Error when deleting bill for: {Username}", username);
            return false;
        }
    }
}