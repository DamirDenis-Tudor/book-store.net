using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

internal class OrderProduct
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }
    
    [Column]
    public int ProductId;
    
    [ForeignKey(nameof(ProductId))] 
    public Product? Product { get; set; }
    
    [Column]
    public int? OrderSessionId { get; init; }

    [ForeignKey(nameof(OrderSessionId))] 
    public OrderSession? OrderSession { get; init; }
    
    [Column]
    [Required] 
    public required int Quantity { get; init; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    [Required]
    public required string OrderProductName { get; init; }
    
}