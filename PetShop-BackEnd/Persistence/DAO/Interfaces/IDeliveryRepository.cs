using Persistence.DTO;
using Persistence.Entity;

namespace Persistence.DAO.Interfaces;

public interface IDeliveryRepository
{
    bool AttachBillToUsername(string username, BillDto bill);
    bool DeleteBillByUsername(string username);
}