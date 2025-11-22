using ConfigFileLibrary.Enums;
using System.Collections;

namespace ConfigFileLibrary.Parsers;

internal class TXTParser(string file) : TokenParser {
    public IEnumerator<(FileToken, string)> GetEnumerator() {

        yield return (FileToken.StartArray, "");
        // yield return (FileToken.PairValue, "Ipsum");
        // yield return (FileToken.KeyValue, "Lorem");
        yield return (FileToken.Primitive, "Lorem");
        yield return (FileToken.EndArray, "");
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}