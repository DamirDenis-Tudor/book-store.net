using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

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
    public decimal Price { get; init; }

    [Column(TypeName = "INT")] 
    [Required] 
    public int Quantity { get; init; }

    [Column(TypeName = "VARBINARY")]
    public byte[]? Photo { get; init; }
}