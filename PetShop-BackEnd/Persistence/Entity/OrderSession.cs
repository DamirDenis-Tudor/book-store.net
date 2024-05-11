using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
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
    public required User User { get; init; }

    [Column(TypeName = "DECIMAL(18, 2)")]
    [Required]
    public required decimal TotalPrice { get; init; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string SessionCode { get; init; }
    
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public IList<OrderProduct>? OrderProducts { get; init; }
}