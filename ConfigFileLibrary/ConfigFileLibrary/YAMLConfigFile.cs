
namespace ConfigFileLibrary;

public class YAMLConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;

    public YAMLConfigFile(string path) {
        string filename = Path.GetFileName(path);

        string[] lines = File.ReadAllLines(path);
        if (lines[0].Contains(':')) {
            // Read lines as dictionary
            option = ReadLinesAsDictionary(lines, filename);
        } else if (lines[0].StartsWith('-')) {
            // Read lines as list
            option = new ArrayConfigOption(new List<IBaseConfigOption>());
        } else {
            // Read lines as primitive
            option = ReadLineAsPrimitive(string.Join("\n", lines));
        }
    }

    public IBaseConfigOption this[string key] => option[key];
    public IBaseConfigOption this[int index] => option[index];
    public bool AsBool() => option.AsBool();
    public List<bool> AsBoolList() => option.AsBoolList();
    public double AsDouble() => option.AsDouble();
    public List<double> AsDoubleList() => option.AsDoubleList();
    public int AsInt() => option.AsInt();
    public List<int> AsIntList() => option.AsIntList();
    public string AsString() => option.AsString();
    public List<string> AsStringList() => option.AsStringList();

    private IBaseConfigOption ReadLineAsPrimitive(string line) {
        return new PrimitiveConfigOption(line);
    }

    private IBaseConfigOption ReadLinesAsDictionary(string[] lines, string filename) {
        Dictionary<string, IBaseConfigOption> start = new Dictionary<string, IBaseConfigOption>();
        foreach (string line in lines) {
            string[] splits = line.Split(':');
            if (splits.Length > 2) {
                throw new FormatException($"Don't include multiple (:) on the same line. Read key: \"{splits[0].Trim()}\"");
            }
            if (splits.Length < 2) {
                throw new FormatException($"No value found for key: \"{splits[0].Trim()}\"");
            }

            start.Add(splits[0].Trim(), ReadLineAsPrimitive(splits[1].Trim()));
        }
        return new ObjectConfigOption(start, filename);
    }

    private IBaseConfigOption ReadLinesAsList(string[] lines) {
        return new ArrayConfigOption(new List<IBaseConfigOption>());
    }
}
