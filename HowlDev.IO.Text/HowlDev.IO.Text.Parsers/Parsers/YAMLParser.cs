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

        int currentIndent = lines[0].indentCount;
        
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
                // Processing object (dictionary)
                string[] splits = line.data.Split(':');
                if (splits.Length > 2) {
                    throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
                }

                string key = splits[0].Replace('-', ' ').Trim();
                yield return (TextToken.KeyValue, key);

                if (string.IsNullOrWhiteSpace(splits[1])) {
                    // Value is on next line(s)
                    if (i + 1 < lines.Count) {
                        if (lines[i + 1].data.StartsWith('-')) {
                            // Next line is an array
                            yield return (TextToken.StartArray, "");
                            structureStack.Push((false, lines[i + 1].indentCount));
                            
                            // Skip ahead through array items
                            i++;
                            while (i < lines.Count && lines[i].data.StartsWith('-')) { 
                                i++; 
                            }
                            i--; // Step back to last array item
                        } else if (lines[i + 1].indentCount > line.indentCount) {
                            // Next line is a nested object (dictionary)
                            yield return (TextToken.StartObject, "");
                            structureStack.Push((true, lines[i + 1].indentCount));
                            
                            // Skip ahead through nested object
                            i++;
                            int targetIndent = line.indentCount;
                            while (i < lines.Count && lines[i].indentCount != targetIndent) { 
                                i++; 
                            }
                            i--; // Step back to last item in nested object
                        }
                    }
                } else {
                    // Inline primitive value
                    yield return (TextToken.Primitive, splits[1].Trim());
                }
            } else {
                // Processing array (list)
                if (currentIndent < line.indentCount && structureStack.Count > 0) {
                    // Nested array within current array
                    yield return (TextToken.StartArray, "");
                    structureStack.Push((false, line.indentCount));
                    
                    // Skip ahead through nested array
                    while (i < lines.Count && lines[i].indentCount != currentIndent) { 
                        i++; 
                    }
                    i--; // Step back
                } else {
                    string lineData = line.data.Replace('-', ' ').Trim();
                    
                    if (string.IsNullOrWhiteSpace(lineData)) {
                        continue;
                    }
                    
                    if (lineData.Contains(':')) {
                        // Array item is an object (dictionary)
                        yield return (TextToken.StartObject, "");
                        structureStack.Push((true, line.indentCount + 1));
                        
                        // Skip ahead through object in array
                        i++;
                        while (i < lines.Count && !lines[i].data.StartsWith('-')) { 
                            i++; 
                        }
                        i--; // Step back
                    } else {
                        // Simple primitive value
                        yield return (TextToken.Primitive, lineData);
                    }
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