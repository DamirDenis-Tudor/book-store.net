/**************************************************************************
 *                                                                        *
 *  Description: OrderSession Entity                                      *
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

using Microsoft.EntityFrameworkCore;

namespace Persistence.Entity;

[Index(nameof(SessionCode), IsUnique=true)]
internal class OrderSession
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }
    
    [Column]
    public int? UserId { get; init; }

    [ForeignKey(nameof(UserId))] 
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public User? User { get; set; }

    [Column(TypeName = "DECIMAL(18, 2)")]
    [Required]
    public decimal TotalPrice { get; set; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string SessionCode { get; init; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string Status { get; init; }
    
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public required IList<OrderProduct> OrderProducts { get; init; }
}