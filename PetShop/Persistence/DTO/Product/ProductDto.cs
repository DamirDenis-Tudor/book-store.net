/**************************************************************************
 *                                                                        *
 *  Description: ProductDto                                               *
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

namespace Persistence.DTO.Product;

public record ProductDto
{
    public required string Name { get; init; }
    public string Description { get; set; } = "";
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required string Category { get; init; }
    public byte[]? Photo { get; set; }
}