using ConfigFileLibrary.Primitives;
using System.Collections.Generic;
namespace ConfigFileLibrary;

/// <summary>
/// This reads a JSON file and parses it into their Config option. 
/// </summary>
public class JSONConfigFile {
    private IBaseConfigOption option;
    
    /// <summary>
    /// Parses in the entire file as a single line and parses it using my own parser. 
    /// </summary>
    public JSONConfigFile(string path) {
        string file = File.ReadAllText(path).Replace('\r', ' ').Replace('\n', ' ');
        option = ParseFileContents(file);
    }

    /// <summary/>
    public IBaseConfigOption this[string key] => option[key];
    /// <summary/>
    public IBaseConfigOption this[int index] => option[index];
    /// <summary/>
    public List<string> AsStringList() => option.AsStringList();
    /// <summary/>
    public List<int> AsIntList() => option.AsIntList();
    /// <summary/>
    public List<double> AsDoubleList() => option.AsDoubleList();
    /// <summary/>
    public List<bool> AsBoolList() => option.AsBoolList();


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
}

