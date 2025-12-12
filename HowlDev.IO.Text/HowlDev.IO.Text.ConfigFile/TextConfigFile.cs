using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
using HowlDev.IO.Text.Parsers;
using HowlDev.IO.Text.Parsers.Enums;
using HowlDev.IO.Text.Parsers.Helpers;

namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// This config file takes in a file path and reads it as either a JSON, YAML, or TXT file.
/// </summary>
public class TextConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;

    /// <summary/>
    public BaseType type => option.type;
    /// <summary/>
    public int Count => option.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => option.Items;
    /// <summary/>
    public IEnumerable<string> Keys => option.Keys;

    /// <summary/>
    public IBaseConfigOption this[string key] => option[key];
    /// <summary/>
    public IBaseConfigOption this[int index] => option[index];
    /// <summary/>
    public bool AsBool() => option.AsBool();
    /// <summary/>
    public List<bool> AsBoolList() => option.AsBoolList();
    /// <summary/>
    public double AsDouble() => option.AsDouble();
    /// <summary/>
    public List<double> AsDoubleList() => option.AsDoubleList();
    /// <summary/>
    public int AsInt() => option.AsInt();
    /// <summary/>
    public List<int> AsIntList() => option.AsIntList();
    /// <summary/>
    public string AsString() => option.AsString();
    /// <summary/>
    public List<string> AsStringList() => option.AsStringList();

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) => option.TryGet(key, out value);
    /// <summary/>
    public bool Contains(string key) => option.Contains(key);

    private List<string> acceptedExtensions = [".txt", ".yml", ".yaml", ".json"];

    private TextConfigFile() {
        option = new PrimitiveConfigOption("");
    }

    /// <summary/>
    public TextConfigFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        if (!acceptedExtensions.Contains(extension)) {
            throw new FormatException($"Extension not recognized: {extension}");
        }

        string file = File.ReadAllText(filePath);
        switch (extension) {
            case ".txt":
                option = ParseFileAsOption(new TXTParser(file));
                return;
            case ".yaml":
            case ".yml":
                List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(file);

                if (lines[0].data.StartsWith('-')) {
                    // Read lines as list
                    option = ReadLinesAsList(lines);
                } else if (lines[0].data.Contains(':')) {
                    // Read lines as dictionary
                    option = ReadLinesAsDictionary(lines);
                } else {
                    // Read lines as primitive
                    string line = "";
                    foreach ((int indentCount, string data) in lines) {
                        line += data + " ";
                    }
                    option = ReadLineAsPrimitive(line);
                }
                break;
            case ".json":
                string fileValue = file.Replace('\r', ' ').Replace('\n', ' ');
                option = ParseFileContents(fileValue);
                break;
            default: throw new Exception("Extension error should be handled above.");
        }
    }

    /// <summary/>
    /// <param name="fileValue">JSON string</param>
    /// <returns></returns>
    public static TextConfigFile ReadTextAsJSON(string fileValue) {
        TextConfigFile file = new TextConfigFile();
        file.option = file.ParseFileContents(fileValue);
        return file;
    }

    private static IBaseConfigOption ParseFileAsOption(TokenParser func) {
        var stack = new Stack<Frame>();
        stack.Push(new Frame(FrameKind.Root));

        foreach (var (type, value) in func) {
            var frame = stack.Peek();
            switch (type) {
                case TextToken.StartObject:
                    stack.Push(new Frame(FrameKind.Object));
                    break;
                case TextToken.EndObject:
                    var obj = stack.Pop();
                    var parent = stack.Peek();
                    parent.Add(obj.AsOption());
                    break;
                case TextToken.StartArray:
                    stack.Push(new Frame(FrameKind.Array));
                    break;
                case TextToken.EndArray:
                    var arr = stack.Pop();
                    parent = stack.Peek();
                    parent.Add(arr.AsOption());
                    break;
                case TextToken.KeyValue:
                    frame.PendingKey = value;
                    break;
                case TextToken.Primitive:
                    frame.Add(new PrimitiveConfigOption(value));
                    break;
                case TextToken.Comment:
                    break;
            }
        }

        Frame root = stack.Pop();
        return root.AsOption();
    }

    #region YAML
    private IBaseConfigOption ReadLinesAsDictionary(List<(int indentCount, string data)> lines) {
        Dictionary<string, IBaseConfigOption> start = new Dictionary<string, IBaseConfigOption>();
        int currentIndent = lines[0].indentCount;
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            string[] splits = line.data.Split(':');
            if (splits.Length > 2) {
                throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
            }

            if (string.IsNullOrWhiteSpace(splits[1])) {
                if (lines[i + 1].data.StartsWith('-')) {
                    start.Add(splits[0].Replace('-', ' ').Trim(), ReadLinesAsList(NextLineLessOrEqual(lines, i + 1)));
                    i++;
                    while (i < lines.Count && lines[i].data.StartsWith('-')) { i++; }
                    i--; // Don't really know what's up with this stuff.
                } else {
                    // Is dictionary
                    start.Add(splits[0].Replace('-', ' ').Trim(), ReadLinesAsDictionary(NextLineLessOrEqual(lines, i + 1)));
                    i++;
                    while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
                    i--;
                }
            } else {
                start.Add(splits[0].Replace('-', ' ').Trim(), ReadLineAsPrimitive(splits[1].Trim()));
            }
        }
        return new ObjectConfigOption(start);
    }

    private IBaseConfigOption ReadLinesAsList(List<(int indentCount, string data)> lines) {
        List<IBaseConfigOption> start = new List<IBaseConfigOption>();
        int currentIndent = lines[0].indentCount;
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            if (currentIndent < line.indentCount) {
                start.Add(ReadLinesAsList(NextLineLessOrEqual(lines, i)));
                while (i < lines.Count && lines[i].indentCount != currentIndent) { i++; }
            } else {
                string lineData = line.data.Replace('-', ' ');
                if (string.IsNullOrWhiteSpace(lineData)) {
                    continue;
                }
                if (lineData.Contains(':')) {
                    start.Add(ReadLinesAsDictionary(NextLineLessOrEqual(lines, i, '-')));
                    i++;
                    while (i < lines.Count && !lines[i].data.StartsWith('-')) { i++; }
                    i--; // Don't really know what's up with this stuff.
                } else {
                    start.Add(ReadLineAsPrimitive(lineData));
                }
            }
        }
        return new ArrayConfigOption(start);
    }

    private List<(int indentCount, string data)> NextLineLessOrEqual(List<(int indentCount, string data)> lines, int index, char startCharacter = '!') {
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

    private IBaseConfigOption ReadLineAsPrimitive(string line) {
        return new PrimitiveConfigOption(line);
    }
    #endregion
    #region JSON

    private IBaseConfigOption ReadAsList(string file, ref int readingIndex) {
        List<IBaseConfigOption> list = new List<IBaseConfigOption>();
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
                list.Add(ReadAsList(file, ref readingIndex));
            } else if (subString.TrimStart().StartsWith('{')) {
                readingIndex = file.IndexOf('{', readingIndex);
                list.Add(ReadAsDictionary(file, ref readingIndex));
            } else {
                list.Add(new PrimitiveConfigOption(subString.Replace(']', ' ')));
                if (nextBracket > nextComma) {
                    readingIndex = nextComma + 1;
                } else {
                    readingIndex = file.IndexOf(']', readingIndex);
                }
            }
        }

        return new ArrayConfigOption(list);
    }

    private IBaseConfigOption ReadAsDictionary(string file, ref int readingIndex) {
        Dictionary<string, IBaseConfigOption> dict = new Dictionary<string, IBaseConfigOption>();
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
                dict.Add(key.Trim(), ReadAsList(file, ref readingIndex));
                continue;
            } else if (value.TrimStart().StartsWith('{')) {
                readingIndex = file.IndexOf('{', readingIndex);
                dict.Add(key.Trim(), ReadAsDictionary(file, ref readingIndex));
                continue;
            } else {
                dict.Add(key.Trim(), new PrimitiveConfigOption(value.Replace('}', ' ')));
            }

            if (breakFlag) {
                readingIndex = nextBracket + 2;
                break;
            }

            readingIndex = nextComma + 1;
        }
        return new ObjectConfigOption(dict);
    }

    private IBaseConfigOption ParseFileContents(string file) {
        int _ = 0; // Just to satisfy the compiler
        if (file.StartsWith("[")) {
            return ReadAsList(file, ref _);
        } else if (file.StartsWith("{")) {
            return ReadAsDictionary(file, ref _);
        } else {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }
    }
    #endregion
}