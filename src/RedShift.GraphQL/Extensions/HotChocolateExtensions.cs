using HotChocolate.AspNetCore.Subscriptions.Protocols.Apollo;
using HotChocolate.AspNetCore.Subscriptions.Protocols;
using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace RedShift.GraphQL.Extensions;

public static class HotChocolateExtensions
{
    private const string TokenRegex = "(?i)Bearer (?<token>.*)";

    public static bool TryParseJwtBearer(this IOperationMessagePayload connectionInitMessage, out string jwtToken)
    {
        jwtToken = default;

        var message = connectionInitMessage as InitializeConnectionMessage;

        if (message?.Payload is null || !message.Payload.HasValue)
        {
            return false;
        }

        var takenSuccessfully = message.Payload.Value.TryGetProperty(HeaderNames.Authorization.ToLower(), out var authHeaderJson);

        if (!takenSuccessfully)
        {
            return false;
        }

        var authHeader = authHeaderJson.GetString();

        if (!Regex.Match(authHeader, TokenRegex).Success)
        {
            return false;
        }

        var tokenPayload = authHeader.Split(' ').LastOrDefault();

        if (string.IsNullOrWhiteSpace(tokenPayload))
        {
            return false;
        }

        jwtToken = tokenPayload.Trim();

        return true;
    }
}
