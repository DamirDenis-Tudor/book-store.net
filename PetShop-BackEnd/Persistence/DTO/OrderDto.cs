namespace Persistence.DTO;

public class OrderDto
{
    public required int Quantity { get; set; }
    public required string? UserName { get; set; }
    public required string ProductName { get; set; }
    public required decimal TotalPrice { get; set; }
}