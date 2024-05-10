using Persistence.DTO;

namespace Persistence.DAO.Interfaces;

public interface IBillRepository
{
    bool AttachBillToUsername(string username, BillDto bill);
    bool DeleteBillByUsername(string username);
}