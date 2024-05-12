namespace Persistence.DTO.Order;

public record OrderSessionDto
{
    public required string Username { get; set; }
    public decimal TotalPrice { get; set; }
    public required string SessionCode { get; set; }
    public required string Status { get; set; }
    public required List<OrderProductDto> OrderProducts { get; set; }
}