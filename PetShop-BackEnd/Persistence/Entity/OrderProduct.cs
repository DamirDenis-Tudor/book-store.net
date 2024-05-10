using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

internal class OrderProduct
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }

    [Required] 
    [Column]
    public int Quantity { get; init; }
    
    [Column]
    public int ProductId;

    [ForeignKey("ProductId")] 
    [Required]
    public required Product Product { get; init; }
    
    [Column]
    public int UserId;

    [ForeignKey("UserId")] 
    [Required] 
    public required User User { get; init; }
}