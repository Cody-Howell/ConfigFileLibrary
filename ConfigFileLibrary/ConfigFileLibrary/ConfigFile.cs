using ConfigFileLibrary.Enums;
using ConfigFileLibrary.Helpers;
using ConfigFileLibrary.Parsers;
using ConfigFileLibrary.Primitives;
using System.Security.AccessControl;

/// <summary>
/// This config file takes in a file path and reads it as either a JSON, YAML, or TXT file.
/// </summary>
public class ConfigFile : IBaseConfigOption {
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

    private ConfigFile() {
        option = new PrimitiveConfigOption("");
    }

    /// <summary/>
    public ConfigFile(string filePath) {
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
                List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(filePath);

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
                option = ParseFileAsOption(new JSONParser(fileValue));
                return;
            default: throw new Exception("Extension error should be handled above.");
        }
    }

    /// <summary/>
    /// <param name="fileValue">JSON string</param>
    /// <returns></returns>
    public static ConfigFile ReadTextAsJSON(string fileValue) {
        ConfigFile file = new ConfigFile();
        file.option = ParseFileAsOption(new JSONParser(fileValue));
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
}