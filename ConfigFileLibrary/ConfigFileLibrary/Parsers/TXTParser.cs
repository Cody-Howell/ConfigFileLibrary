using ConfigFileLibrary.Enums;
using System.Collections;

namespace ConfigFileLibrary.Parsers;

internal class TXTParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        yield return (TextToken.StartObject, "");

        char split = ':';
        string[] fileLines = file.Split('\n');
        for (int i = 0; i < fileLines.Length; i++) {
            if (String.IsNullOrWhiteSpace(fileLines[i])) continue;

            string[] things = fileLines[i].Split(split);
            if (things.Length > 2) throw new FormatException($"More than 1 split character was found at line {i + 1}. Please remove it or change your character to something else.");
            if (things.Length == 1) throw new FormatException($"No split character was found at line {i + 1}. Please add it or override your custom split character.");

            if (things[1].Contains("[")) {
                string longString = things[1];
                try {
                    if (!longString.Contains("]")) {
                        while (!longString.Contains("]")) {
                            i++;
                            longString += file[i];

                            if (longString.Contains(split)) {
                                throw new Exception();
                            }
                        }
                    }
                } catch {
                    throw new FormatException($"Error parsing array around line {i + 1}. Please ensure you have a closing array brace.");
                }

                longString = longString.Replace("[", "").Replace("]", "");
                string[] arrayValues = longString.Split(',');
                yield return (TextToken.KeyValue, things[0].Trim());
                yield return (TextToken.StartArray, "");

                foreach (string value in arrayValues) {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    yield return (TextToken.Primitive, value);
                }
                yield return (TextToken.EndArray, "");
            } else {
                yield return (TextToken.KeyValue, things[0].Trim());
                yield return (TextToken.Primitive, things[1].Trim());
            }
        }
        yield return (TextToken.EndObject, "");
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}