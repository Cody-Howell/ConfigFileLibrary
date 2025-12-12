using HowlDev.IO.Text.Parsers.Enums;
using HowlDev.IO.Text.Parsers.Helpers;
using System.Collections;

namespace HowlDev.IO.Text.Parsers;

public class YAMLParser(string file) : TokenParser {
    public IEnumerator<(TextToken, string)> GetEnumerator() {
        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file);
        Stack<bool> isObjectStack = new();

        if (lines[0].data.StartsWith('-')) {
            yield return (TextToken.StartArray, "");
            isObjectStack.Push(false);
        } else if (lines[0].data.Contains(':')) {
            yield return (TextToken.StartObject, "");
            isObjectStack.Push(true);
        } else {
            string line = "";
            foreach ((int indentCount, string data) in lines) {
                line += data + " ";
            }
            yield return (TextToken.Primitive, line.Trim());
            yield break;
        }

        int currentIndent = 0;


    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}