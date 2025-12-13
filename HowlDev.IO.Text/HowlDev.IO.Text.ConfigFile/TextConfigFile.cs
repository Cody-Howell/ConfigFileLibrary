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

    #region Option Exports
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
                option = ParseFileAsOption(new YAMLParser(file));
                break;
            case ".json":
                option = ParseFileAsOption(new JSONParser(file));
                break;
            default: throw new Exception("Extension error should be handled above. Extension was not recognized.");
        }
    }

    /// <summary/>
    /// <param name="fileValue">JSON string</param>
    public static TextConfigFile ReadTextAsJSON(string fileValue) {
        TextConfigFile file = new TextConfigFile();
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
}