using ConfigFileLibrary.Enums;
using ConfigFileLibrary.Parsers;
using ConfigFileLibrary.Primitives;

/// <summary>
/// This config file takes in a file path and reads it as either a JSON, YAML, or TXT file.
/// </summary>
public class ConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;
    #region IBaseConfigOption Implementation
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
#endregion

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
                option = ParseFileAsOption(new YAMLParser(file));
                return;
            case ".json":
                string fileValue = file.Replace('\r', ' ').Replace('\n', ' ');
                option = ParseFileAsOption(new JSONParser(fileValue));
                return;
            default: throw new Exception("Extension error should be handled above.");
        }
    }

    /// <summary/>
    /// <param name="fileValue">String</param>
    /// <param name="type">Type the string should be parsed as</param>
    /// <returns></returns>
    public static ConfigFile ReadTextAs(string fileValue, FileTypes type) {
        ConfigFile file = new ConfigFile();
        switch (type) {
            case FileTypes.JSON:  file.option = ParseFileAsOption(new JSONParser(fileValue)); break;
            case FileTypes.YAML: file.option = ParseFileAsOption(new YAMLParser(fileValue)); break;
            case FileTypes.TXT: file.option = ParseFileAsOption(new TXTParser(fileValue)); break;
        };
        return file;
    }

    private static IBaseConfigOption ParseFileAsOption(TokenParser func) {
        var stack = new Stack<Frame>();
        stack.Push(new Frame(FrameKind.Root));

        //List<(TextToken, string)> tokens = new(func);

        foreach (var (type, value) in func) {
            var frame = stack.Peek();
            switch (type) {
                case TextToken.StartObject:
                    stack.Push(new Frame(FrameKind.Object));
                    break;
                case TextToken.EndObject:
                    var obj = stack.Pop();
                    if (obj.Kind != FrameKind.Object) {
                        throw new FormatException("Mismatched end object token.");
                    }
                    var parent = stack.Peek();
                    parent.Add(obj.AsOption());
                    break;
                case TextToken.StartArray:
                    stack.Push(new Frame(FrameKind.Array));
                    break;
                case TextToken.EndArray:
                    var arr = stack.Pop();
                    if (arr.Kind != FrameKind.Array) {
                        throw new FormatException("Mismatched end array token.");
                    }
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
}