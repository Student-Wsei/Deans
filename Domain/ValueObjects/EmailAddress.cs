using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class EmailAddress : IEquatable<EmailAddress>
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public string User { get; }
    public string Domain { get; }
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email nie może być pusty.", nameof(value));
        }
        var trimmed = value.Trim();
        if (!EmailRegex.IsMatch(trimmed))
        {
            throw new ArgumentException($"Nieprawidłowy format adresu email: {value}", nameof(value));
        }
        var parts = trimmed.Split('@');
        User = parts[0];
        Domain = parts[1].ToLowerInvariant();
        Value = $"{User}@{Domain}";
    }

    public static EmailAddress Parse(string value) => new(value);

    public static bool TryParse(string? value, out EmailAddress? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value)) return false;
        try
        {
            result = new EmailAddress(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string Format() => Value;

    public override string ToString() => Value;

    public bool Equals(EmailAddress? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as EmailAddress);

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(EmailAddress email) => email.Value;
    public static explicit operator EmailAddress(string value) => new(value);
}
