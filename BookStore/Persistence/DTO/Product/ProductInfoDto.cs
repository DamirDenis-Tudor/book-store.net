namespace Persistence.DTO.Product;

public class ProductInfoDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public string? Link { get; init; } = "";
}