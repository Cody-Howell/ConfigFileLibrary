using ConfigFileLibrary.Enums;
namespace ConfigFileLibrary.Parsers;

internal interface TokenParser : IEnumerable<(FileToken, string)>;