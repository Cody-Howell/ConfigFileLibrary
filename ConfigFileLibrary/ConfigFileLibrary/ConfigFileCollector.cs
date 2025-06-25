namespace ConfigFileLibrary;

/// <summary>
/// This class takes in an array of file paths and allows you to retrieve various different
/// <c>ConfigFile</c> outputs. You can mix and match any of TXT, YAML, or JSON files.
/// </summary>
public class ConfigFileCollector {
    Dictionary<string, TXTConfigFile> txtFiles = new();
    Dictionary<string, YAMLConfigFile> yamlFiles = new();
    Dictionary<string, JSONConfigFile> jsonFiles = new();

    /// <summary>
    /// Given a list of file paths, checks and validates extensions and non-duplication 
    /// per file type. Throws errors for invalid or missing extensions and for duplicate 
    /// filenames within extension types.
    /// </summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public ConfigFileCollector(List<string> filenames) {
        foreach (string s in filenames) {
            if (!Path.HasExtension(s)) throw new FormatException($"File {s} does not have an extension.");
            string extension = Path.GetExtension(s);
            switch (extension) {
                case ".txt":
                    if (txtFiles.ContainsKey(Path.GetFileNameWithoutExtension(s)))
                        throw new NotSupportedException("Cannot add in two filenames of the same name and extension.");
                    txtFiles.Add(Path.GetFileNameWithoutExtension(s), new TXTConfigFile(s)); 
                    break;
                case ".json":
                    if (jsonFiles.ContainsKey(Path.GetFileNameWithoutExtension(s)))
                        throw new NotSupportedException("Cannot add in two filenames of the same name and extension.");
                    jsonFiles.Add(Path.GetFileNameWithoutExtension(s), new JSONConfigFile(s)); 
                    break;
                case ".yml": 
                case ".yaml":
                    if (yamlFiles.ContainsKey(Path.GetFileNameWithoutExtension(s)))
                        throw new NotSupportedException("Cannot add in two filenames of the same name and extension.");
                    yamlFiles.Add(Path.GetFileNameWithoutExtension(s), new YAMLConfigFile(s)); 
                    break;
                default: throw new FormatException($"Extension not recognized: {extension}");
            }
        }
    }

    /// <summary>
    /// Given a file name (WITHOUT extension or folder navigation), returns 
    /// a TXTConfigFile.
    /// </summary>
    /// <exception cref="FileNotFoundException"/>
    public TXTConfigFile GetTXTFile(string filename) {
        try {
            return txtFiles[filename];
        } catch {
            List<string> keys = new (txtFiles.Select(v => v.Key));
            throw new FileNotFoundException($"Filename does not exist. Available keys: \n\t{String.Join("\n\t", keys)}");
        }
    }

    /// <summary>
    /// Given a file name (WITHOUT extension or folder navigation), returns 
    /// a JSONConfigFile.
    /// </summary>
    /// <exception cref="FileNotFoundException"/>
    public JSONConfigFile GetJSONFile(string filename) {
        try {
            return jsonFiles[filename];
        } catch {
            List<string> keys = new(jsonFiles.Select(v => v.Key));
            throw new FileNotFoundException($"Filename does not exist. Available keys: \n\t{String.Join("\n\t", keys)}");
        }
    }

    /// <summary>
    /// Given a file name (WITHOUT extension or folder navigation), returns 
    /// a YAMLConfigFile.
    /// </summary>
    /// <exception cref="FileNotFoundException"/>
    public YAMLConfigFile GetYAMLFile(string filename) {
        try {
            return yamlFiles[filename];
        } catch {
            List<string> keys = new(yamlFiles.Select(v => v.Key));
            throw new FileNotFoundException($"Filename does not exist. Available keys: \n\t{String.Join("\n\t", keys)}");
        }
    }
}
