using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Persistence.Entity;

[Index(nameof(SessionCode), IsUnique=true)]
internal class OrderSession
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }
    
    [Column]
    public int UserId { get; init; }

    [ForeignKey(nameof(UserId))] 
    [Required] 
    public User? User { get; set; }

    [Column(TypeName = "DECIMAL(18, 2)")]
    [Required]
    public decimal TotalPrice { get; set; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string SessionCode { get; init; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string Status { get; init; }
    
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public required IList<OrderProduct> OrderProducts { get; init; }
}