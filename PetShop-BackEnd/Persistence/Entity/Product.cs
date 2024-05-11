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
    public required decimal Price { get; set; }

    [Column(TypeName = "INT")] 
    [Required] 
    public required int Quantity { get; set; }

    [Column(TypeName = "VARBINARY")]
    public byte[]? Photo { get; init; }

    public IList<OrderProduct>? OrderProducts { get; init; }
}