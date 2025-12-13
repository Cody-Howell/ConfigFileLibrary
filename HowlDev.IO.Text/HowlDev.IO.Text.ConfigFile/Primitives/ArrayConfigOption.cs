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

    /// <inheritdoc/>
    public TypeCode GetTypeCode() => throw new InvalidOperationException("GetTypeCode not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => throw new InvalidOperationException("ToBoolean not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => throw new InvalidOperationException("ToByte not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => throw new InvalidOperationException("ToChar not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidOperationException("ToDateTime not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => throw new InvalidOperationException("ToDecimal not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => throw new InvalidOperationException("ToDouble not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToInt16 not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToInt32 not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToInt64 not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => throw new InvalidOperationException("ToSByte not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => throw new InvalidOperationException("ToSingle not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => throw new InvalidOperationException("ToString not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => throw new InvalidOperationException("ToType not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt16 not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt32 not allowed on type of ArrayConfigOption.");

    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt64 not allowed on type of ArrayConfigOption.");
}