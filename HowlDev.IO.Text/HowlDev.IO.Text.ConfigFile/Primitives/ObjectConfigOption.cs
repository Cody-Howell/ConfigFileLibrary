using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ObjectConfigOption : IBaseConfigOption {
    private Dictionary<string, IBaseConfigOption> obj = new Dictionary<string, IBaseConfigOption>();
    private string resourcePath;

    /// <summary/>
    public BaseType type => BaseType.Object;
    /// <summary/>
    public int Count => obj.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => throw new InvalidOperationException("Item enumeration not allowed on type of ObjectConfigOption.");
    /// <summary/>
    public IEnumerable<string> Keys => obj.Keys;

    /// <summary/>
    public ObjectConfigOption(Dictionary<string, IBaseConfigOption> obj, string parentPath = "", string myPath = "") {
        this.obj = new(obj);
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }
    /// <summary/>
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
    /// <summary/>
    public IBaseConfigOption this[int index] => throw new InvalidOperationException("Operation invalid on type of ObjectConfigOption.");
    /// <summary/>
    public string AsString() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    /// <summary/>
    public int AsInt() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    /// <summary/>
    public double AsDouble() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    /// <summary/>
    public bool AsBool() => throw new InvalidOperationException("Type casting not allowed on type ObjectConfigOption");
    /// <summary/>
    public List<string> AsStringList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    /// <summary/>
    public List<int> AsIntList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    /// <summary/>
    public List<double> AsDoubleList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");
    /// <summary/>
    public List<bool> AsBoolList() => throw new InvalidOperationException("List returning not allowed on type ObjectConfigOption");

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) {
        return obj.TryGetValue(key, out value!); // I'm just passing it through. 
    }

    /// <summary/>
    public bool Contains(string key) {
        return obj.ContainsKey(key);
    }
}