/**************************************************************************
 *                                                                        *
 *  Description: BillDetails Entity                                       *
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

[Index(nameof(UserId), IsUnique = true)]
internal class BillDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Column]
    [Required]
    public int? UserId { get; init; }

    [ForeignKey(nameof(UserId))] 
    public User? User { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(100)]
    public string Address { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string Telephone { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string Country { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string City { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public string PostalCode { get; set; } = "";
}