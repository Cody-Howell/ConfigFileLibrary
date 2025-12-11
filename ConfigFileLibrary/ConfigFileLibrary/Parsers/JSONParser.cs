using ConfigFileLibrary.Enums;
using System.Collections;

namespace ConfigFileLibrary.Parsers;

internal class JSONParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        int readingIndex = 0;
        bool isObject;

        if (file.StartsWith("[")) {
            yield return (TextToken.StartArray, "");
            isObject = false;
        } else if (file.StartsWith("{")) {
            yield return (TextToken.StartObject, "");
            isObject = true;
        } else {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }

        int nextComma, nextBracket;
        string subString, key, value;
        bool breakFlag = false;
        while (readingIndex != 0) {
            nextComma = file.IndexOf(',', readingIndex);
            nextBracket = file.IndexOf(']', readingIndex);

            if (isObject) {
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
                        yield return (TextToken.EndObject, "");
                        continue;
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
                    yield return (TextToken.EndObject, "");
                    continue;
                }

                if (value.TrimStart().StartsWith('[')) {
                    readingIndex = file.IndexOf('[', readingIndex);
                    yield return (TextToken.KeyValue, key.Trim());
                    yield return (TextToken.StartArray, "");
                    continue;
                } else if (value.TrimStart().StartsWith('{')) {
                    readingIndex = file.IndexOf('{', readingIndex);
                    yield return (TextToken.KeyValue, key.Trim());
                    yield return (TextToken.StartObject, "");
                    continue;
                } else {
                    yield return (TextToken.KeyValue, key.Trim());
                    yield return (TextToken.Primitive, value.Replace('}', ' '));
                }

                if (breakFlag) {
                    readingIndex = nextBracket + 2;
                    yield return (TextToken.EndObject, "");
                    continue;
                }

                readingIndex = nextComma + 1;
            } else {
                if (readingIndex >= nextBracket) {
                    yield return (TextToken.EndArray, "");
                    continue;
                }

                if (nextComma < 0) {
                    subString = file[readingIndex..nextBracket].Replace('"', ' '); // Uses a "range" operator for the substring
                } else {
                    subString = file[readingIndex..nextComma].Replace('"', ' '); // Uses a "range" operator for the substring
                }

                if (subString.TrimStart().StartsWith('[')) {
                    readingIndex = file.IndexOf('[', readingIndex);
                    yield return (TextToken.StartArray, "");
                } else if (subString.TrimStart().StartsWith('{')) {
                    readingIndex = file.IndexOf('{', readingIndex);
                    yield return (TextToken.StartObject, "");
                } else {
                    yield return (TextToken.Primitive, subString.Replace(']', ' '));
                    if (nextBracket > nextComma) {
                        readingIndex = nextComma + 1;
                    } else {
                        readingIndex = file.IndexOf(']', readingIndex);
                    }
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}