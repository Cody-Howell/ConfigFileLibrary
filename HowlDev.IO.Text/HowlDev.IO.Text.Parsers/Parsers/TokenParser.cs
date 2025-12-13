using HowlDev.IO.Text.Parsers.Enums;
namespace HowlDev.IO.Text.Parsers;

/// <summary>
/// Returns tokens following the format listed in the README.
/// </summary>
public interface TokenParser : IEnumerable<(TextToken, string)>;