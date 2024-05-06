using PetShop_BackEnd.Persistence.Model;

namespace PetShop_BackEnd.Persistence.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string Name { get; init; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string Password { get; init; }
    
    [Column]
    [Required]
    [EmailAddress]
    [StringLength(30)]
    public required string Email { get; init; }

    [Column]
    [Required]
    [StringLength(100)]
    public required string UserType { get; init; }

    [ForeignKey("BillingDetailsId")] 
    public required BillDetails? BillDetails { get; init; }
    
    [Column]
    public int? BillingDetailsId { get; init; }
}






