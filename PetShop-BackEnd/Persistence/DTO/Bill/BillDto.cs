/**************************************************************************
 *                                                                        *
 *  Description: BillDto                                                  *
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

namespace Persistence.DTO.Bill;

public record BillDto
{
    public required string Address { get; init; } = "";
    public required string Telephone { get; init; } = "";
    public required string Country { get; init; } = "";
    public required string City { get; init; } = "";
    public required string PostalCode { get; init; } = "";
}