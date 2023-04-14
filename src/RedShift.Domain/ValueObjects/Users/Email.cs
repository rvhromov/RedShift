using RedShift.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace RedShift.Domain.ValueObjects.Users;

public sealed record Email
{
    private const string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

    public Email(string value)
    {
        if (!Validate(value))
        {
            throw new ValidationException("Invalid email address.");
        }

        Value = value;
    }

    public string Value { get; }

    private static bool Validate(string emailAddress) => 
        Regex.IsMatch(emailAddress, EmailPattern, RegexOptions.IgnoreCase);

    public static implicit operator string(Email email) =>
        email.Value;

    public static implicit operator Email(string email) =>
        new(email);
}
