using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using Persistence.DAO.Interfaces;
using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Repositories;

internal class DeliveryRepository(PersistenceAccess.DatabaseContext dbContext): IDeliveryRepository
{
    public bool AttachBillToUsername(string username, BillDto bill)
    {
            /*try
            {
                dbContext.Bills.Add(bill);
                dbContext.SaveChanges();
            
                var user = dbContext.Users
                    .Include(user => user.BillDetails)
                    .FirstOrDefault(u => u.Username == username);
                if (user == null) return false;

                user.BillDetails = bill;
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }*/
            throw new NotImplementedException();
    }

    public bool DeleteBillByUsername(string username)
    {
        throw new NotImplementedException();
    }
}