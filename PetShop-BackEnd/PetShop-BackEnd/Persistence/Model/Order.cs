using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetShop_BackEnd.Persistence.Entities;

namespace PetShop_BackEnd.Persistence.Model;

[Table("Order")]
public class Order
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
    public Product? Product { get; init; }
    
    [Column]
    public int UserId;

    [ForeignKey("UserId")] 
    [Required] 
    public User? User { get; init; }
}