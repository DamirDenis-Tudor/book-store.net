namespace Business.DTO;

public record OrderItemDto
{
    public required string ProductName { get; init; }
    public required int OrderQuantity { get; init; }
}