namespace Persistence.DTO;

public record BillDto
{
    public required string Address { get; set; }
    public required string Telephone { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
}