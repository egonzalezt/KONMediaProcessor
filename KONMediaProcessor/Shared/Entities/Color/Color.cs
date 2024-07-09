namespace KONMediaProcessor.Shared.Entities.Color;

using Exceptions;
using System.Text.RegularExpressions;

public partial class Color : ValueObject
{
    private static readonly Regex HexColorRegex = ColorHexRegex();
    public string Value { get; }
    private const string HexColorKey = "#";

    public Color(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException("The color cannot be null or empty.");
        }

        if (value.StartsWith(HexColorKey) && !HexColorRegex.IsMatch(value))
        {
            throw new ColorHexNotValidException($"The color '{value}' is not a valid HEX color.");
        }

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Color color)
    {
        return color.Value;
    }

    public static implicit operator Color(string value)
    {
        return new Color(value);
    }

    [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", RegexOptions.Compiled)]
    private static partial Regex ColorHexRegex();
}
