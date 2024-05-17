/**************************************************************************
 *                                                                        *
 *  Description: Product Entity                                           *
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

[Index(nameof(Name), IsUnique = true)]
internal class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    [Required]
    public required string Name { get; init; }

    [Column(TypeName = "DECIMAL(18, 2)")]
    [Required]
    public required decimal Price { get; set; }

    [Column(TypeName = "INT")] [Required] public required int Quantity { get; set; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    [Required]
    public required string Category { get; init; }

    [Column(TypeName = "VARBINARY")] public byte[]? Photo { get; init; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public required IList<OrderProduct> OrderProducts { get; init; }
}