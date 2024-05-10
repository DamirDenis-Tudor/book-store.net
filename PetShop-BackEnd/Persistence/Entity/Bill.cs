using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

internal class Bill
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(100)]
    public required string Address { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string Telephone { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string Country { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string City { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public required string PostalCode { get; init; }
}