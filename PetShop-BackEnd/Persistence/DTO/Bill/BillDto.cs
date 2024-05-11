namespace Persistence.DTO.Bill;

public record BillDto
{
    public string Address { get; init; } = "";
    public string Telephone { get; init; } = "";
    public string Country { get; init; } = "";
    public string City { get; init; } = "";
    public string PostalCode { get; init; } = "";
}