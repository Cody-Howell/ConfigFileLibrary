﻿using System.ComponentModel;

namespace ConfigFileLibrary.Helpers;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class YAMLHelper {
    public static List<(int, string)> ReturnOrderedLines(string path) {
        List<(int, string)> lines = new List<(int, string)>();
        string[] fileLines = File.ReadAllLines(path);
        for (int i = 0; i < fileLines.Length; i++) {
            lines.Add((CountIndexes(fileLines[i]), fileLines[i].Trim()));
        }
        return lines;
    }

    private static int CountIndexes(string line) {
        int count = 0;
        for (int i = 0; i < line.Length - 3; i+=4) {
            if (!char.IsWhiteSpace(line[i])) { break; }
            if ((line[i] == '\t') ||
                string.IsNullOrWhiteSpace(line.Substring(i, 4))) {
                count++;
            }
        }
        return count;
    }
}
