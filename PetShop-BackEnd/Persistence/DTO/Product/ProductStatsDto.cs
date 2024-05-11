namespace Persistence.DTO.Product;

public class ProductStatsDto
{
    public required string Name { get; init; }
    public required decimal TotalRevenue { get; init; }
    public required int TotalItemsSold { get; init; }
    public byte[]? Photo { get; set; }
}