using ConfigFileLibrary.Enums;
using ConfigFileLibrary.Helpers;
using System.Collections;

namespace ConfigFileLibrary.Parsers;

internal class YAMLParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file.Split('\n'));
        bool isObject;

        if (lines[0].data.StartsWith('-')) {
            yield return (TextToken.StartArray, "");
            isObject = false;
        } else if (lines[0].data.Contains(':')) {
            yield return (TextToken.StartObject, "");
            isObject = true;
        } else {
            // Read lines as primitive
            string line = "";
            foreach ((int indentCount, string data) in lines) {
                line += data + " ";
            }
            yield return (TextToken.Primitive, line);
            yield break;
        }

        int currentIndent = lines[0].indentCount;
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            if (isObject) {

                string[] splits = line.data.Split(':');
                if (splits.Length > 2) {
                    throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
                }

                if (string.IsNullOrWhiteSpace(splits[1])) {
                    if (lines[i + 1].data.StartsWith('-')) {
                        yield return (TextToken.KeyValue, splits[0].Replace('-', ' ').Trim());
                        yield return (TextToken.StartArray, "");
                        i++;
                        while (i < lines.Count && lines[i].data.StartsWith('-')) { i++; }
                        i--; // Don't really know what's up with this stuff.
                    } else {
                        // Is dictionary
                        yield return (TextToken.KeyValue, splits[0].Replace('-', ' ').Trim());
                        yield return (TextToken.StartObject, "");
                        i++;
                        while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
                        i--;
                    }
                } else {
                    yield return (TextToken.KeyValue, splits[0].Replace('-', ' ').Trim());
                    yield return (TextToken.StartObject, splits[1].Trim());
                }
            } else {
                if (currentIndent < line.indentCount) {
                    yield return (TextToken.StartArray, "");
                    while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
                } else {
                    string lineData = line.data.Replace('-', ' ');
                    if (string.IsNullOrWhiteSpace(lineData)) {
                        continue;
                    }
                    if (lineData.Contains(':')) {
                        yield return (TextToken.StartObject, "");
                        i++;
                        while (i < lines.Count && !lines[i].data.StartsWith('-')) { i++; }
                        i--; // Don't really know what's up with this stuff.
                    } else {
                        yield return (TextToken.Primitive, lineData);
                    }
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}