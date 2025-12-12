using HowlDev.IO.Text.Parsers.Enums;
namespace HowlDev.IO.Text.Parsers;

public interface TokenParser : IEnumerable<(TextToken, string)>;