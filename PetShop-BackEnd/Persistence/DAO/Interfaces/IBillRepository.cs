using Persistence.DTO;
using Persistence.DTO.Bill;

namespace Persistence.DAO.Interfaces;

public interface IBillRepository
{
    bool UpdateBillToUsername(string username, BillDto bill);
}