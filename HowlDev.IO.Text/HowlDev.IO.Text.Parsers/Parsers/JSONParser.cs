using HowlDev.IO.Text.Parsers.Enums;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

/// <summary/>
public class JSONParser(string file) : TokenParser {
    private int readingIndex = 0;
    /// <summary/>
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        string fileValue = file.Replace('\r', ' ').Replace('\n', ' ');

        foreach (var item in ParseFileContents(fileValue)) yield return item;
    }

    private IEnumerable<(TextToken, string)> ReadAsList(string file) {
        yield return (TextToken.StartArray, "");
        readingIndex++;
        int nextComma = 0;
        int nextBracket = 1;

        string subString = "";
        while (readingIndex != 0) {
            nextComma = file.IndexOf(',', readingIndex);
            nextBracket = file.IndexOf(']', readingIndex);

            if (readingIndex >= nextBracket) {
                readingIndex += 2;
                break;
            }

            if (nextComma < 0) {
                subString = file[readingIndex..nextBracket].Replace('"', ' '); // Uses a "range" operator for the substring
            } else {
                subString = file[readingIndex..nextComma].Replace('"', ' '); // Uses a "range" operator for the substring
            }

            if (subString.TrimStart().StartsWith('[')) {
                readingIndex = file.IndexOf('[', readingIndex);
                foreach (var item in ReadAsList(file)) yield return item;
            } else if (subString.TrimStart().StartsWith('{')) {
                readingIndex = file.IndexOf('{', readingIndex);
                foreach (var item in ReadAsDictionary(file)) yield return item;
            } else {
                string primitive = subString.Replace(']', ' ').Trim();
                if (!string.IsNullOrEmpty(primitive)) {
                    yield return (TextToken.Primitive, primitive);
                }
                if (nextBracket > nextComma) {
                    readingIndex = nextComma + 1;
                } else {
                    readingIndex = file.IndexOf(']', readingIndex);
                }
            }
        }
        yield return (TextToken.EndArray, "");
    }

    private IEnumerable<(TextToken, string)> ReadAsDictionary(string file) {
        yield return (TextToken.StartObject, "");
        readingIndex++;
        int nextComma = 0;
        int nextBracket = 0;

        string subString = "";
        string key = "";
        string value = "";
        bool breakFlag = false;
        while (readingIndex != 0) {
            nextComma = file.IndexOf(',', readingIndex);
            nextBracket = file.IndexOf('}', readingIndex);

            if (nextComma < 0) {
                subString = file[readingIndex..nextBracket].Replace('"', ' '); // Uses a "range" operator for the substring
                int colonIndex = subString.IndexOf(':');
                key = subString[0..colonIndex];
                colonIndex++;
                value = subString[colonIndex..];
            } else {
                subString = file[readingIndex..nextComma].Replace('"', ' ');
                int colonIndex = subString.IndexOf(':');
                if (colonIndex < 0) {
                    readingIndex = nextComma + 1;
                    break;
                }
                key = subString[0..colonIndex];
                colonIndex++;
                if (nextComma > nextBracket) {
                    breakFlag = true;
                    value = subString[colonIndex..(subString.Length - 1)];
                } else {
                    value = subString[colonIndex..];
                }
            }

            if (readingIndex >= nextBracket) {
                readingIndex += 2;
                break;
            }

            if (value.TrimStart().StartsWith('[')) {
                readingIndex = file.IndexOf('[', readingIndex);
                yield return (TextToken.KeyValue, key.Trim());
                foreach (var item in ReadAsList(file)) yield return item;
                continue;
            } else if (value.TrimStart().StartsWith('{')) {
                readingIndex = file.IndexOf('{', readingIndex);
                yield return (TextToken.KeyValue, key.Trim());
                foreach (var item in ReadAsDictionary(file)) yield return item;
                continue;
            } else {
                yield return (TextToken.KeyValue, key.Trim());
                yield return (TextToken.Primitive, value.Replace('}', ' ').Trim());
            }

            if (breakFlag) {
                readingIndex = nextBracket + 2;
                break;
            }

            readingIndex = nextComma + 1;
        }
        yield return (TextToken.EndObject, "");
    }

    private IEnumerable<(TextToken, string)> ParseFileContents(string file) {
        if (file.StartsWith("[")) {
            foreach (var item in ReadAsList(file)) yield return item;
        } else if (file.StartsWith("{")) {
            foreach (var item in ReadAsDictionary(file)) yield return item;
        } else {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}