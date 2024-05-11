namespace Persistence.DTO.Order;

public class OrderSessionDto
{
    public required string Username { get; set; }
    public required decimal TotalPrice { get; set; }
    public required string SessionCode { get; set; }
    public required List<OrderProductDto> OrderProducts { get; set; }
}