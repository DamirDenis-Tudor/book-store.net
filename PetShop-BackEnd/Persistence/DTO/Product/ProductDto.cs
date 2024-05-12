namespace Persistence.DTO.Product;

public record ProductDto
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public byte[]? Photo { get; set; }
}