using ConfigFileLibrary.Enums;
using System.Collections;

#pragma warning disable 1591
public class TXTParser(string fileValue): IEnumerable<(FileToken, string)> {

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