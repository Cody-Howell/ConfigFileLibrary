using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ArrayConfigOption : IBaseConfigOption {
    private List<IBaseConfigOption> array = new List<IBaseConfigOption>();
    private string resourcePath;

    /// <summary/>
    public BaseType type => BaseType.Array;
    /// <summary/>
    public int Count => array.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => array;
    /// <summary/>
    public IEnumerable<string> Keys => throw new InvalidOperationException("Key enumeration not allowed on type of ArrayConfigOption.");

    /// <summary/>
    public ArrayConfigOption(List<IBaseConfigOption> array, string parentPath = "", string myPath = "") {
        this.array = array;
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }

    /// <summary/>
    public IBaseConfigOption this[string key] => throw new InvalidOperationException("Operation invalid on type of ArrayConfigOption.");
    /// <summary/>
    public IBaseConfigOption this[int index] {
        get {
            if (index < 0 || index >= array.Count) {
                string error = $"Index {index} is out of range. This array has {array.Count} items.";
                if (resourcePath.Length >= 3) error += $"\n\tPath: {resourcePath}";
                throw new IndexOutOfRangeException(error);
            }

            return array[index];
        }
    }
    /// <summary/>
    public string AsString() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    /// <summary/>
    public int AsInt() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    /// <summary/>
    public double AsDouble() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    /// <summary/>
    public bool AsBool() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    /// <summary/>
    public List<string> AsStringList() {
        List<string> outList = new List<string>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsString());
        }
        return outList;
    }
    /// <summary/>
    public List<int> AsIntList() {
        List<int> outList = new List<int>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsInt());
        }
        return outList;
    }
    /// <summary/>
    public List<double> AsDoubleList() {
        List<double> outList = new List<double>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsDouble());
        }
        return outList;
    }
    /// <summary/>
    public List<bool> AsBoolList() {
        List<bool> outList = new List<bool>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsBool());
        }
        return outList;
    }

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) => throw new InvalidOperationException("TryGet not allowed on type of ArrayConfigOption.");

    /// <summary/>
    public bool Contains(string key) => throw new InvalidOperationException("Contains not allowed on type of ArrayConfigOption.");
}