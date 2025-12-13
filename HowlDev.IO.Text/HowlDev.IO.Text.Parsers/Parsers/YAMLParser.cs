using HowlDev.IO.Text.Parsers.Enums;
using HowlDev.IO.Text.Parsers.Helpers;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

/// <summary/>
/// <param name="file"></param>
public class YAMLParser(string file) : TokenParser {
    /// <summary/>
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file);

        if (lines[0].data.StartsWith('-')) {
            foreach (var item in ReadLinesAsList(lines)) yield return item;
        } else if (lines[0].data.Contains(':')) {
            foreach (var item in ReadLinesAsDictionary(lines)) yield return item;
        } else {
            string line = "";
            foreach ((int indentCount, string data) in lines) {
                line += data + " ";
            }
            yield return (TextToken.Primitive, line.Trim());
            yield break;
        }
    }

    private IEnumerable<(TextToken, string)> ReadLinesAsDictionary(List<(int indentCount, string data)> lines) {
        yield return (TextToken.StartObject, "");
        int currentIndent = lines[0].indentCount;
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            string[] splits = line.data.Split(':');
            if (splits.Length > 2) {
                throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
            }

            if (string.IsNullOrWhiteSpace(splits[1])) {
                yield return (TextToken.KeyValue, splits[0].Replace('-', ' ').Trim());
                if (lines[i + 1].data.StartsWith('-')) {
                    foreach (var item in ReadLinesAsList(NextLineLessOrEqual(lines, i + 1))) yield return item;
                    i++;
                    while (i < lines.Count && lines[i].data.StartsWith('-')) { i++; }
                    i--; // Don't really know what's up with this stuff.
                } else {
                    foreach (var item in ReadLinesAsDictionary(NextLineLessOrEqual(lines, i + 1))) yield return item;
                    i++;
                    while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
                    i--;
                }
            } else {
                yield return (TextToken.KeyValue, splits[0].Replace('-', ' ').Trim());
                yield return (TextToken.Primitive, splits[1].Trim());
            }
        }
        yield return (TextToken.EndObject, "");
    }

    private IEnumerable<(TextToken, string)> ReadLinesAsList(List<(int indentCount, string data)> lines) {
        yield return (TextToken.StartArray, "");
        int currentIndent = lines[0].indentCount;
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            if (currentIndent < line.indentCount) {
                foreach (var item in ReadLinesAsList(NextLineLessOrEqual(lines, i))) yield return item;
                i++;
                while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
                i--;
            } else {
                string lineData = line.data.Replace('-', ' ');
                if (string.IsNullOrWhiteSpace(lineData)) {
                    continue;
                }
                if (lineData.Contains(':')) {
                    foreach (var item in ReadLinesAsDictionary(NextLineLessOrEqual(lines, i, '-'))) yield return item;
                    i++;
                    while (i < lines.Count && !lines[i].data.StartsWith('-')) { i++; }
                    i--; // Don't really know what's up with this stuff.
                } else {
                    yield return (TextToken.Primitive, lineData.Trim());
                }
            }
        }
        yield return (TextToken.EndArray, "");
    }

    private static List<(int indentCount, string data)> NextLineLessOrEqual(List<(int indentCount, string data)> lines, int index, char startCharacter = '!') {
        List<(int, string)> outList = new List<(int, string)>();
        int indentCount = lines[index].indentCount;
        for (int i = index; i < lines.Count; i++) {
            if (lines[i].indentCount < indentCount || (i != index && lines[i].data.StartsWith(startCharacter))) {
                break;
            }
            outList.Add(lines[i]);
        }
        return outList;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}