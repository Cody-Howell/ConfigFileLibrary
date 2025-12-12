using HowlDev.IO.Text.Parsers.Enums;
using HowlDev.IO.Text.Parsers.Helpers;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

public class YAMLParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file.Split('\n'));
        
        if (lines.Count == 0) {
            yield break;
        }

        Stack<(bool isObject, int indentLevel)> structureStack = new();
        bool isObject;

        if (lines[0].data.StartsWith('-')) {
            yield return (TextToken.StartArray, "");
            structureStack.Push((false, lines[0].indentCount));
            isObject = false;
        } else if (lines[0].data.Contains(':')) {
            yield return (TextToken.StartObject, "");
            structureStack.Push((true, lines[0].indentCount));
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

        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            
            // Close any structures that have ended (indent level decreased)
            while (structureStack.Count > 0 && line.indentCount < structureStack.Peek().indentLevel) {
                var (wasObject, _) = structureStack.Pop();
                if (wasObject) {
                    yield return (TextToken.EndObject, "");
                } else {
                    yield return (TextToken.EndArray, "");
                }
            }

            // Update current context
            if (structureStack.Count > 0) {
                isObject = structureStack.Peek().isObject;
            }

            if (isObject) {
                string[] splits = line.data.Split(':');
                if (splits.Length > 2) {
                    throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
                }

                string key = splits[0].Replace('-', ' ').Trim();
                yield return (TextToken.KeyValue, key);

                if (string.IsNullOrWhiteSpace(splits[1])) {
                    // Check what comes next
                    if (i + 1 < lines.Count) {
                        if (lines[i + 1].data.StartsWith('-')) {
                            // Next line is an array
                            yield return (TextToken.StartArray, "");
                            structureStack.Push((false, lines[i + 1].indentCount));
                        } else if (lines[i + 1].indentCount > line.indentCount) {
                            // Next line is a nested object
                            yield return (TextToken.StartObject, "");
                            structureStack.Push((true, lines[i + 1].indentCount));
                        }
                    }
                } else {
                    // Inline value
                    yield return (TextToken.Primitive, splits[1].Trim());
                }
            } else {
                // We're in an array
                string lineData = line.data.Replace('-', ' ').Trim();
                
                if (string.IsNullOrWhiteSpace(lineData)) {
                    continue;
                }
                
                if (lineData.Contains(':')) {
                    // Array item is an object
                    yield return (TextToken.StartObject, "");
                    structureStack.Push((true, line.indentCount + 1));
                } else if (i + 1 < lines.Count && lines[i + 1].indentCount > line.indentCount) {
                    // Array item has nested structure
                    yield return (TextToken.StartArray, "");
                    structureStack.Push((false, lines[i + 1].indentCount));
                } else {
                    // Simple primitive value
                    yield return (TextToken.Primitive, lineData);
                }
            }
        }

        // Close any remaining open structures
        while (structureStack.Count > 0) {
            var (wasObject, _) = structureStack.Pop();
            if (wasObject) {
                yield return (TextToken.EndObject, "");
            } else {
                yield return (TextToken.EndArray, "");
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}