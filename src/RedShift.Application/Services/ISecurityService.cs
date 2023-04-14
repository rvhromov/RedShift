using RedShift.Application.Dtos.Security;

namespace RedShift.Application.Services;

public interface ISecurityService
{
    public bool PasswordMatch(string plainPassword, string plainConfirmPassword);
    public HashedPasswordDto HashPassword(string plainPassword);
    public bool VerifyPassword(string plainPassword, string hash, string salt);
}
