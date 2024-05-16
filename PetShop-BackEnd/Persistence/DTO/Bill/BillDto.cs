namespace Persistence.DTO.Bill;

public record BillDto
{
    public required string Address { get; init; } = "";
    public required string Telephone { get; init; } = "";
    public required string Country { get; init; } = "";
    public required string City { get; init; } = "";
    public required string PostalCode { get; init; } = "";
}