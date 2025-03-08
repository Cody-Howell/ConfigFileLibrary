using ConfigFileLibrary.Primitives;
using System.Collections.Generic;
namespace ConfigFileLibrary;

public class JSONConfigFile {
    private IBaseConfigOption option;
    public JSONConfigFile(string path) {
        string file = File.ReadAllText(path).Replace('\r', ' ').Replace('\n', ' ');
        int _ = 1; // Just to satisfy the compiler
        if (file.StartsWith("[")) {
            option = ReadAsList(file, ref _);
        } else if (file.StartsWith("{")) {
            option = ReadAsDictionary(file, ref _);
        } else {
            throw new InvalidDataException("JSON file must start with either [ or {");
        }
    }

    public IBaseConfigOption this[string key] => option[key];
    public IBaseConfigOption this[int index] => option[index];
    public List<string> AsStringList() => option.AsStringList();
    public List<int> AsIntList() => option.AsIntList();
    public List<double> AsDoubleList() => option.AsDoubleList();
    public List<bool> AsBoolList() => option.AsBoolList();


    private IBaseConfigOption ReadAsList(string file, ref int readingIndex) {
        List<IBaseConfigOption> list = new List<IBaseConfigOption>();
        int nextComma = file.IndexOf(',', readingIndex);
        int nextBracket = file.IndexOf(']', readingIndex);

        string subString = "";
        while (nextBracket > nextComma && readingIndex != 0) {
            if (nextComma < 0) {
                subString = file[readingIndex..nextBracket].Replace('"', ' '); // Uses a "range" operator for the substring
            } else {
                subString = file[readingIndex..nextComma].Replace('"', ' '); // Uses a "range" operator for the substring
            }
            list.Add(new PrimitiveConfigOption(subString));

            readingIndex = nextComma + 1;
            nextComma = file.IndexOf(',', readingIndex);
        }

        return new ArrayConfigOption(list);
    }

    private IBaseConfigOption ReadAsDictionary(string file, ref int readingIndex) {
        Dictionary<string, IBaseConfigOption> dict = new Dictionary<string, IBaseConfigOption>();
        int nextComma = file.IndexOf(',', readingIndex);
        int nextBracket = file.IndexOf('}', readingIndex);

        string subString = "";
        string key = "";
        string value = "";
        while (nextBracket > nextComma && readingIndex != 0) {
            if (nextComma < 0) {
                subString = file[readingIndex..nextBracket].Replace('"', ' '); // Uses a "range" operator for the substring
                int colonIndex = subString.IndexOf(':');
                key = subString[0..colonIndex];
                colonIndex++;
                value = subString[colonIndex..];
            } else {
                subString = file[readingIndex..nextComma].Replace('"', ' ');
                int colonIndex = subString.IndexOf(':');
                key = subString[0..colonIndex];
                colonIndex++;
                value = subString[colonIndex..];
            }


            dict.Add(key.Trim(), new PrimitiveConfigOption(value));

            readingIndex = nextComma + 1;
            nextComma = file.IndexOf(',', readingIndex);
        }
        return new ObjectConfigOption(dict);
    }
}

