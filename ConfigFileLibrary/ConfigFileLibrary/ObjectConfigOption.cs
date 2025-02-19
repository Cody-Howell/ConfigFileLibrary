namespace ConfigFileLibrary;

public class ObjectConfigOption : IBaseConfigOption {
    private Dictionary<string, IBaseConfigOption> obj = new Dictionary<string, IBaseConfigOption>();
    private IBaseConfigOption? parent;

    public IBaseConfigOption? Parent { get => parent; }

    public ObjectConfigOption(Dictionary<string, IBaseConfigOption> obj, IBaseConfigOption? parent = null) {
        this.obj = new (obj);
        this.parent = parent;
    }

    public IBaseConfigOption this[string key] {
        get {
            if (!obj.TryGetValue(key, out var value))
                throw new KeyNotFoundException($"File did not contain key \"{key}\".");

            try {
                return value;
            } catch (KeyNotFoundException ex) {
                throw new KeyNotFoundException($"\tat key '{key}'\n{ex.Message}", ex);
            } catch (IndexOutOfRangeException ex) {
                throw new IndexOutOfRangeException($"\tat key '{key}'\n{ex.Message}", ex);
            }
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
