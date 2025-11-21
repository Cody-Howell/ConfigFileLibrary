

using ConfigFileLibrary.Helpers;
using ConfigFileLibrary.Primitives;

/// <summary/>
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

    private ConfigFile() {
        option = new PrimitiveConfigOption("");
    }

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
            case ".json":
                string fileValue = File.ReadAllText(file).Replace('\r', ' ').Replace('\n', ' ');
                option = ParseFileContents(fileValue);
                break;
            default: throw new FormatException($"Extension not recognized: {extension}");
        }
        // option  = new PrimitiveConfigOption(" ");
    }

    public static ConfigFile ReadTextAsJSON(string fileValue) {
        ConfigFile file = new ConfigFile();
        file.option = file.ParseFileContents(fileValue);
        return file;
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