//using System;

//namespace ConfigFileLibrary;

///// <summary>
///// This reads a standard TXT file for single-depth configuration options. It holds an internal dictionary 
///// to retreive the values and has helper methods for converting to Ints, Doubles, and Bools. <br/>
///// A sample file is below: <br/>
///// <c>
///// Enemy Name: Bad Guy <br/>
///// Enemy Color: #9645ff <br/>
///// Enemy Damage: 23.4 <br/>
///// Enemy Speed: 15 <br/>
///// Is Boss: true
///// </c>
///// </summary>
//public class TXTConfigFile {
//    private Dictionary<string, (int, IBaseConfigOption)> values = new Dictionary<string, (int, IBaseConfigOption)>();

//    /// <summary>
//    /// Returns a string array of valid keys.
//    /// </summary>
//    public string[] Keys => values.Keys.ToArray();

//    /// <summary>
//    /// Retreives Key-Value pairs from the given file, one on each line. Keys and values are trimmed
//    /// before inserting into the internal dictionary. <br/>
//    /// 
//    /// Throws helpful exceptions (line numbers) with split character values, if more than 1 or 0 split characters were found.
//    /// </summary>
//    /// <param name="path">Relative or absolute path to the file</param>
//    /// <param name="split">Character to split key/value pairs by</param>
//    /// <exception cref="FormatException"></exception>
//    public TXTConfigFile(string path, char split = ':') {
//        string[] file = File.ReadAllLines(path);
//        for (int i = 0; i < file.Length; i++) {
//            string[] things = file[i].Split(split);
//            if (things.Length > 2) throw new FormatException($"More than 1 split character was found at line {i + 1}. Please remove it or change your character to something else.");
//            if (things.Length == 1) throw new FormatException($"No split character was found at line {i + 1}. Please add it or override your custom split character.");
//            values.Add(things[0].Trim(), (i, new PrimitiveConfigOption(things[1].Trim())));
//        }
//    }

//    /// <summary>
//    /// Returns the value as a string.
//    /// </summary>
//    /// <param name="key">The trimmed key from the file</param>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public string AsString(string key) {
//        return GetValue(key);
//    }

//    /// <summary>
//    /// Returns the value as an Int. Throws an exception if it can't be parsed.
//    /// </summary>
//    /// <param name="key">The trimmed key from the file</param>
//    /// <exception cref="NotSupportedException"></exception>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public int AsInt(string key) {
//        int value;
//        bool succeeded = int.TryParse(GetValue(key), out value);
//        if (succeeded) {
//            return value;
//        } else {
//            throw new NotSupportedException($"This string (line {values[key].Item1 + 1}) can't be parsed to an Int.");
//        }
//    }

//    /// <summary>
//    /// Returns the value as a Double. Throws an exception if it can't be parsed.
//    /// </summary>
//    /// <param name="key">The trimmed key from the file</param>
//    /// <exception cref="NotSupportedException"></exception>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public double AsDouble(string key) {
//        double value;
//        bool succeeded = double.TryParse(GetValue(key), out value);
//        if (succeeded) {
//            return value;
//        } else {
//            throw new NotSupportedException($"This string (line {values[key].Item1 + 1}) can't be parsed to a Double.");
//        }
//    }

//    /// <summary>
//    /// Returns the value as a Boolean. Throws an exception if it can't be parsed. <br/>
//    /// The only valid values are <c>True</c>, <c>False</c>, <c>true</c>, and <c>false</c>.
//    /// </summary>
//    /// <param name="key">The trimmed key from the file</param>
//    /// <exception cref="NotSupportedException"></exception>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public bool AsBool(string key) {
//        bool value;
//        bool succeeded = bool.TryParse(GetValue(key), out value);
//        if (succeeded) {
//            return value;
//        } else {
//            throw new NotSupportedException($"This string (line {values[key].Item1 + 1}) can't be parsed to a Boolean.");
//        }
//    }

//    private string GetValue(string key) {
//        try {
//            return values[key].Item2;
//        } catch {
//            throw new KeyNotFoundException($"Key (\"{key}\") was not found in the file.");
//        }
//    }
//}
