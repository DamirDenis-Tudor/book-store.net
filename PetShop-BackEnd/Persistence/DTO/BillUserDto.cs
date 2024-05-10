namespace Persistence.DTO;

public record BillUserDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string? Address { get; set; }
    public required string? Telephone { get; set; }
    public required string? Country { get; set; }
    public required string? City { get; set; }
    public required string? PostalCode { get; set; }
}