using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entity;

internal class OrderSession
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; init; }
    
    [Column]
    public int? UserId { get; init; }

    [ForeignKey("UserId")] 
    [Required] 
    public required User User { get; init; }

    [Column(TypeName = "DECIMAL(18, 2)")]
    [Required]
    public required decimal TotalPrice { get; init; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public required string SessionCode { get; init; }
    
}