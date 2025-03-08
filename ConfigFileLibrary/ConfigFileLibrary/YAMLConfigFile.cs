﻿using ConfigFileLibrary.Helpers;
using ConfigFileLibrary.Primitives;
namespace ConfigFileLibrary;

public class YAMLConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;

    public YAMLConfigFile(string path) {
        string filename = Path.GetFileName(path);

        List<(int indentCount, string data)> lines = YAMLHelper.ReturnOrderedLines(path);

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

    private IBaseConfigOption ReadLinesAsDictionary(List<(int indentCount, string data)> lines) {
        Dictionary<string, IBaseConfigOption> start = new Dictionary<string, IBaseConfigOption>();
        for (int i = 0; i < lines.Count; i++) {
            (int indentCount, string data) line = lines[i];
            string[] splits = line.data.Split(':');
            if (splits.Length > 2) {
                throw new FormatException($"Don't include multiple (:) on the same line. I read key: \"{splits[0].Trim()}\"");
            }
            if (splits.Length < 2 && line.indentCount >= lines[i+1].indentCount) {
                throw new FormatException($"No value found for key: \"{splits[0].Trim()}\"");
            }

            start.Add(splits[0].Replace('-', ' ').Trim(), ReadLineAsPrimitive(splits[1].Trim()));
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
}
