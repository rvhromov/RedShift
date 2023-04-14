using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedShift.Application.Helpers;
using RedShift.Application.Services;
using RedShift.Domain.Enums;
using RedShift.Infrastructure.Options;

namespace RedShift.Infrastructure.Services;

internal class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IConfiguration configuration)
    {
        _jwtOptions = configuration.GetOptions<JwtOptions>(nameof(JwtOptions));
    }

    public string CreateAccessToken(Guid userId, RoleType userRole)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, userRole.ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes);

        var tokenOptions = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, default, expires, signInCredentials);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }
}
