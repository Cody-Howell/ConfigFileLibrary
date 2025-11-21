

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
}