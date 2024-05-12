namespace Persistence.DTO.Order;

public record OrderProductDto
{
    public required string ProductName { get; init; }
    public decimal Price { get; init; }
    public required string SessionCode { get; init; }
    public required int Quantity { get; init; }
}