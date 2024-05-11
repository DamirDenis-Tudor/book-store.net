namespace Persistence.DTO.Order;

public class OrderProductDto
{
    public required string ProductName { get; init; }
    public required decimal Price { get; init; }
    public required string SessionCode { get; init; }
    public required int Quantity { get; init; }
}