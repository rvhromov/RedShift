namespace RedShift.Application.Dtos.Security;

public sealed class HashedPasswordDto
{
    public string Password { get; set; }
    public string Salt { get; set; }
}
