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
    public required string FirstName { get; init; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string LastName { get; init; }

    [Column(TypeName = "VARCHAR(20)")]
    [Required]
    public required string Username { get; init; }

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

    [ForeignKey("BillDetailsId")] 
    public BillDetails? BillDetails { get; set; }

    [Column] 
    public int? BillDetailsId { get; init; }
}