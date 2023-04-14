using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.AspNetCore.Subscriptions.Protocols;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RedShift.GraphQL.Extensions;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using RedShift.Infrastructure.Options;
using RedShift.Application.Helpers;

namespace RedShift.GraphQL.Middlewares;

public sealed class SubscriptionAuthMiddleware : DefaultSocketSessionInterceptor
{
    private const string RejectMessage = "Unauthorized.";
    private const string ConnectionExpMessage = "Connection expired.";

    private readonly int _jwtLifetimeInMin;
    private readonly JwtBearerOptions _jwtOptions;

    public SubscriptionAuthMiddleware(IConfiguration configuration, JwtBearerOptions jwtOptions)
    {
        _jwtLifetimeInMin = configuration.GetOptions<JwtOptions>(nameof(JwtOptions)).ExpiresInMinutes;
        _jwtOptions = jwtOptions;
    }

    public override async ValueTask<ConnectionStatus> OnConnectAsync(
        ISocketSession session, 
        IOperationMessagePayload connectionInitMessage, 
        CancellationToken cancellationToken = default)
    {
		try
        {
            if (!connectionInitMessage.TryParseJwtBearer(out var jwtToken))
            {
                return ConnectionStatus.Reject(RejectMessage);
            }

            var claims = ValidateJwtToken(jwtToken, _jwtOptions);

            if (claims is null)
            {
                return ConnectionStatus.Reject(RejectMessage);
            }

            InitializeIdentity(session, claims);
            StartConnectionExpirationCountdown(session, _jwtLifetimeInMin, cancellationToken);
        }
        catch (Exception exception)
		{
            return ConnectionStatus.Reject(exception.Message);
        }

        return ConnectionStatus.Accept();
    }

    private ClaimsPrincipal ValidateJwtToken(string jwtToken, JwtBearerOptions jwtOptions)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal claims = null;

        try
        {
            claims = tokenHandler.ValidateToken(jwtToken, jwtOptions.TokenValidationParameters, out var validatedToken);
        }
        catch (Exception)
        {
            return claims;
        }

        return claims;
    }

    private static void InitializeIdentity(ISocketSession session, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? default;
        var userRole = claimsPrincipal.FindFirstValue(ClaimTypes.Role) ?? default;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, userRole)
        };

        session.Connection.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
    }

    private static void StartConnectionExpirationCountdown(ISocketSession session, int jwtLifetimeInMin, CancellationToken cancellationToken)
    {
        async Task closeOnExp()
        {
            var delay = (int)TimeSpan.FromMinutes(jwtLifetimeInMin).TotalMilliseconds;

            await Task.Delay(delay, cancellationToken);
            await session.Connection.CloseAsync(ConnectionExpMessage, ConnectionCloseReason.NormalClosure, cancellationToken);
        }

        Task.Run(closeOnExp, cancellationToken);
    }
}
