using System;

namespace ConfigFileLibrary;

public class ObjectConfigOption : IBaseConfigOption {
    private Dictionary<string, IBaseConfigOption> obj = new Dictionary<string, IBaseConfigOption>();
    private string resourcePath;

    public ObjectConfigOption(Dictionary<string, IBaseConfigOption> obj, string parentPath = "", string myPath = "") {
        this.obj = new(obj);
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }

    public IBaseConfigOption this[string key] {
        get {
            if (!obj.TryGetValue(key, out var value)) {
                string error = $"Object does not contain key \"{key}\".";
                if (resourcePath.Length >= 3) error += $"\n\tPath: {resourcePath}";
                throw new KeyNotFoundException(error);
            }

            return value;
        }
    }

    public IBaseConfigOption this[int index] => throw new InvalidOperationException("Operation invalid on type of ObjectConfigOption.");

    public string AsString() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    public int AsInt() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    public double AsDouble() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    public bool AsBool() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    public List<string> AsStringList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    public List<int> AsIntList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    public List<double> AsDoubleList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    public List<bool> AsBoolList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
}
