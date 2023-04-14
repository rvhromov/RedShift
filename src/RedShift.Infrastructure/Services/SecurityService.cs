using Microsoft.Extensions.Configuration;
using RedShift.Application.Dtos.Security;
using RedShift.Application.Helpers;
using RedShift.Application.Services;
using RedShift.Infrastructure.Options;
using System.Security.Cryptography;
using System.Text;

namespace RedShift.Infrastructure.Services;

internal sealed class SecurityService : ISecurityService
{
    private readonly SecurityOptions _securityOptions;

    public SecurityService(IConfiguration configuration) =>
        _securityOptions = configuration.GetOptions<SecurityOptions>(nameof(SecurityOptions));

    public bool PasswordMatch(string plainPassword, string plainConfirmPassword) =>
        plainPassword == plainConfirmPassword;

    public HashedPasswordDto HashPassword(string plainPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(_securityOptions.KeySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(plainPassword),
            salt,
            _securityOptions.Interactions,
            HashAlgorithmName.SHA512,
            _securityOptions.KeySize);

        return new HashedPasswordDto
        {
            Password = Convert.ToHexString(hash),
            Salt = Convert.ToBase64String(salt)
        };
    }

    public bool VerifyPassword(string plainPassword, string hash, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);

        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            plainPassword,
            saltBytes,
            _securityOptions.Interactions,
            HashAlgorithmName.SHA512,
            _securityOptions.KeySize);

        return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
    }
}
