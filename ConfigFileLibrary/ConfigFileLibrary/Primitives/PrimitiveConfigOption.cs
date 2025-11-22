using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class PrimitiveConfigOption : IBaseConfigOption {
    private string value;
    /// <summary/>
    public PrimitiveConfigOption(string value) {
        this.value = value.Trim();
    }
    /// <summary/>
    public IBaseConfigOption this[string key] => throw new InvalidOperationException("Key indexing operation invalid on type of PrimitiveConfigOption.");
    /// <summary/>
    public IBaseConfigOption this[int index] => throw new InvalidOperationException("List indexing operation invalid on type of PrimitiveConfigOption.");
    /// <summary/>
    public string AsString() {
        return value;
    }
    /// <summary/>
    public int AsInt() {
        int outValue;
        bool succeeded = int.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to an Int.");
        }
    }
    /// <summary/>
    public double AsDouble() {
        double outValue;
        bool succeeded = double.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Double.");
        }
    }
    /// <summary/>
    public bool AsBool() {
        bool outValue;
        bool succeeded = bool.TryParse(value, out outValue);
        if (succeeded) {
            return outValue;
        } else {
            throw new InvalidCastException($"Value \"{value}\" is not castable to a Boolean.");
        }
    }
    /// <summary/>
    public List<string> AsStringList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    /// <summary/>
    public List<int> AsIntList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    /// <summary/>
    public List<double> AsDoubleList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    /// <summary/>
    public List<bool> AsBoolList() => throw new InvalidOperationException("List returning not allowed on type PrimitiveConfigOption");
    /// <summary/>
    public override string ToString() {
        return value;
    }
}