/**************************************************************************
 *                                                                        *
 *  Description: IBillRepository                                          *
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

using Common;
using Persistence.DTO.Bill;

namespace Persistence.DAO.Interfaces;

/// <summary>
/// Interface for Bill Repository operations.
/// </summary>
public interface IBillRepository
{
    /// <summary>
    /// Updates the billing details for a given username.
    /// </summary>
    /// <param name="username">The username to update the billing details for.</param>
    /// <param name="bill">The new billing details.</param>
    /// <returns>A Result containing either a success message or a DaoErrorType indicating the type of error.</returns>
    Result<VoidResult, DaoErrorType> UpdateBillByUsername(string username, BillDto bill);

    /// <summary>
    /// Retrieves the billing details for a given username.
    /// </summary>
    /// <param name="username">The username to retrieve the billing details for.</param>
    /// <returns>A Result containing either the BillDto or a DaoErrorType indicating the type of error.</returns>
    Result<BillDto, DaoErrorType> GetBillingDetails(string username);
}