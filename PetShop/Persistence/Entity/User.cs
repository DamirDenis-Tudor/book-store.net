/**************************************************************************
 *                                                                        *
 *  Description: User Entity                                              *
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

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
internal class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string FirstName { get; set; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string LastName { get; set; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string Username { get; set; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string Password { get; set; }

    [Column]
    [Required]
    [EmailAddress]
    [StringLength(30)]
    public required string Email { get; set; }

    [Column]
    [Required]
    [StringLength(100)]
    public required string UserType { get; init; }


    [DeleteBehavior(DeleteBehavior.Cascade)]
    public required BillDetails BillDetails { get; init; }
    
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public IList<OrderSession>? OrderSessions { get; init; } = new List<OrderSession>();
}