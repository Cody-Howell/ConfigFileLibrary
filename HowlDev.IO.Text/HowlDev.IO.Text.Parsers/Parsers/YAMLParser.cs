using HowlDev.IO.Text.Parsers.Enums;
using HowlDev.IO.Text.Parsers.Helpers;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

public class YAMLParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file);

        if (lines[0].data.StartsWith('-')) {
            yield return (TextToken.StartArray, "");
            // option = ReadLinesAsList(lines);
        } else if (lines[0].data.Contains(':')) {
            // Read lines as dictionary
            yield return (TextToken.StartObject, "");
            // option = ReadLinesAsDictionary(lines);
        } else {
            // Read lines as primitive
            string line = "";
            foreach ((int indentCount, string data) in lines) {
                line += data + " ";
            }
            yield return (TextToken.Primitive, line.Trim());
            yield break;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}