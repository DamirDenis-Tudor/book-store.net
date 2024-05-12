using Logger;
using Persistence.DTO;
using Persistence.DTO.Bill;

namespace Persistence.DAO.Interfaces;

public interface IBillRepository
{
    Result<string, DaoErrorType> UpdateBillToUsername(string username, BillDto bill);
}