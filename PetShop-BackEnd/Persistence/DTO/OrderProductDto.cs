namespace Persistence.DTO;

public class OrderProductDto
{
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required string SessionCode { get; set; }
}