namespace Business.DTO;

public record OrderDto
{
    public required string Username { get; init; }
    public required List<OrderItemDto> OrderItemDtos { get; init; }
}