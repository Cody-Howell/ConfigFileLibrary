using HowlDev.IO.Text.Parsers.Enums;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

public class YAMLParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}