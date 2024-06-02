/**************************************************************************
 *                                                                        *
 *  Description: OrderProduct Entity                                      *
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

internal class OrderProduct
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }
    
    [Column]
    public int? ProductId;
    
    [ForeignKey(nameof(ProductId))] 
    public Product? Product { get; set; }
    
    [Column]
    public int? OrderSessionId { get; init; }

    [ForeignKey(nameof(OrderSessionId))] 
    public OrderSession? OrderSession { get; init; }
    
    [Column]
    public int? ProductInfoId { get; init; }

    [ForeignKey(nameof(ProductInfoId))] 
    [Required]
    public required ProductInfo ProductInfo { get; set; }
    
    [Column]
    [Required] 
    public required decimal OrderTimePrice { get; init; }
    
    [Column]
    [Required] 
    public required int Quantity { get; init; }
    
}