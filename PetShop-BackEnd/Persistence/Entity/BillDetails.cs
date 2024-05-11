using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Entity;

[Index(nameof(UserId), IsUnique = true)]
internal class BillDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Column]
    public int? UserId { get; init; }

    [ForeignKey(nameof(UserId))] 
    [Required] 
    public User? User { get; init; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(100)]
    public string Address { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string Telephone { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string Country { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string City { get; set; } = "";

    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public string PostalCode { get; set; } = "";
}