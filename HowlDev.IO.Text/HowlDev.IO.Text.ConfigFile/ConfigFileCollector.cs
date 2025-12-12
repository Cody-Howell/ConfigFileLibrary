namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// This class takes in an array of file paths and allows you to retrieve various different
/// <c>ConfigFile</c> outputs. You can mix and match any of TXT, YAML, or JSON files.
/// </summary>
public class ConfigFileCollector {
    private Dictionary<string, TextConfigFile> files = new();

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
            if (files.ContainsKey(Path.GetFileName(s)))
                throw new NotSupportedException("Cannot add in two filenames of the same name and extension.");
            files.Add(Path.GetFileName(s), new TextConfigFile(s));
        }
    }

    /// <summary>
    /// Given a file name (WITHOUT folder navigation), returns 
    /// a ConfigFile.
    /// </summary>
    /// <exception cref="FileNotFoundException"/>
    public TextConfigFile GetFile(string filename) {
        try {
            return files[filename];
        } catch {
            List<string> keys = new(files.Select(v => v.Key));
            throw new FileNotFoundException($"Filename does not exist. Available keys: \n\t{string.Join("\n\t", keys)}");
        }
    }
}