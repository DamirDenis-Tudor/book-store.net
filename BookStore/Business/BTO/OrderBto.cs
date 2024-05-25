namespace Business.BTO;

public record OrderBto
{
    public required string Username { get; set; }
    public required List<OrderItemBto> OrderItemBtos { get; init; }
}