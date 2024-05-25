namespace Business.BTO;

public record UserLoginBto
{
    public required string  Username { get; init; }
    public required string  Password { get; init; }
}