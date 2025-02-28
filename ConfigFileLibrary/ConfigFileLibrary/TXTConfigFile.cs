using ConfigFileLibrary.Primitives;
namespace ConfigFileLibrary;

/// <summary>
/// This reads a standard TXT file for single-depth configuration options. It holds an internal <see cref="ObjectConfigOption"/>
/// to retreive the values and has helper methods for converting to Ints, Doubles, and Bools. <br/>
/// A sample file is below: <br/>
/// <c>
/// Enemy Name: Bad Guy <br/>
/// Enemy Color: #9645ff <br/>
/// Enemy Damage: 23.4 <br/>
/// Enemy Speed: 15 <br/>
/// Is Boss: true
/// </c>
/// </summary>
public class TXTConfigFile {
    private ObjectConfigOption obj;

    /// <summary>
    /// Retreives Key-Value pairs from the given file, one on each line. Keys and values are trimmed
    /// before inserting into the internal dictionary. <br/>
    /// 
    /// Throws helpful exceptions (line numbers) with split character values, if more than 1 or no split characters were found.
    /// </summary>
    /// <param name="path">Relative or absolute path to the file</param>
    /// <param name="split">Character to split key/value pairs by</param>
    /// <exception cref="FormatException"></exception>
    public TXTConfigFile(string path, char split = ':') {
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
                    if (String.IsNullOrWhiteSpace(value)) continue;
                    array.Add(new PrimitiveConfigOption(value.Trim()));
                }
                values.Add(things[0].Trim(), new ArrayConfigOption(array));
            } else {
                values.Add(things[0].Trim(), new PrimitiveConfigOption(things[1].Trim()));
            }
        }

        string filename = Path.GetFileName(path);
        obj = new ObjectConfigOption(values, filename);
    }

    /// <summary>
    /// Direct indexer into the internal object.
    /// </summary>
    public IBaseConfigOption this[string key] => obj[key];
}
