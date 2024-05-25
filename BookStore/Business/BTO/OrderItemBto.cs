namespace Business.BTO;

public record OrderItemBto
{
    public required string ProductName { get; init; }
    public required int OrderQuantity { get; init; }
}