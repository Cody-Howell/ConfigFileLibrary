

using ConfigFileLibrary.Helpers;
using ConfigFileLibrary.Primitives;

public class ConfigFile {
    private IBaseConfigOption option;

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
    public ConfigFile(string file) {
        string extension = Path.GetExtension(file);
        switch (extension) {
            case ".txt":
                option = ReadAsTXTFile(file);
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
            default:
                throw new Exception($"Extension {extension} was not known.");
        }
        // option  = new PrimitiveConfigOption(" ");
    }


    # region TXT
    private IBaseConfigOption ReadAsTXTFile(string path) {
        char split = ':';
        string[] file = File.ReadAllLines(path);
        Dictionary<string, IBaseConfigOption> values = new Dictionary<string, IBaseConfigOption>();
        for (int i = 0; i < file.Length; i++) {
            if (String.IsNullOrWhiteSpace(file[i])) continue;

            string[] things = file[i].Split(split);
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
                List<IBaseConfigOption> array = new List<IBaseConfigOption>();
                foreach (string value in arrayValues) {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    array.Add(new PrimitiveConfigOption(value.Trim()));
                }
                values.Add(things[0].Trim(), new ArrayConfigOption(array));
            } else {
                values.Add(things[0].Trim(), new PrimitiveConfigOption(things[1].Trim()));
            }
        }

        string filename = Path.GetFileName(path);
        return new ObjectConfigOption(values, filename);
    }
    #endregion

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