namespace Persistence.DTO;

public record BillUserDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required BillDto? BillDto { get; set; }
}