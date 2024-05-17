/**************************************************************************
 *                                                                        *
 *  Description: OrderSessionDto                                          *
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

namespace Persistence.DTO.Order;

public record OrderSessionDto
{
    public required string Username { get; init; }
    public decimal TotalPrice { get; set; }
    public required string SessionCode { get; init; }
    public required string Status { get; init; }
    public required List<OrderProductDto> OrderProducts { get; init; }
}