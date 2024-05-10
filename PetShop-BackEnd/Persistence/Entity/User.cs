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

    [ForeignKey("BillDetailsId")] public required Bill? BillDetails { get; set; }

    [Column] public int? BillDetailsId { get; init; }

    public override string ToString()
    {
        return $"User: Id={Id}, FirstName={FirstName}, LastName={LastName}, Username={Username}, Email={Email}, UserType={UserType}";
    }
}