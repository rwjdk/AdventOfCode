namespace Utils;

public record PrefixWithInteger(string PrefixText, int Number, string RestOfString);

public record Prefix(string PrefixText, string RestOfString);

public enum ValueAndIdentiferOrder
{
    ValueIdentifer,
    IdentifierValue
}