
namespace ConfigFileLibrary;

public class PrimitiveConfigOption : IBaseConfigOption {
    private string value;
    private IBaseConfigOption? parent;

    public IBaseConfigOption? Parent { get => parent; }

    public PrimitiveConfigOption(string value, IBaseConfigOption? parent = null) {
        this.value = value.Trim();
        this.parent = parent;
    }

    public IBaseConfigOption this[string key] => throw new InvalidOperationException("Operation invalid on type of PrimitiveConfigOption.");

    public IBaseConfigOption this[int index] => throw new InvalidOperationException("Operation invalid on type of PrimitiveConfigOption.");

    public string AsString() {
        return value;
    }

    public int AsInt() {
        int outValue;
        bool succeeded = int.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to an Int.");
        }
    }

    public double AsDouble() {
        double outValue;
        bool succeeded = double.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Double.");
        }
    }

    public bool AsBool() {
        bool outValue;
        bool succeeded = bool.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Boolean.");
        }
    }

    public List<string> AsStringList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    public List<int> AsIntList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    public List<double> AsDoubleList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    public List<bool> AsBoolList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
}
