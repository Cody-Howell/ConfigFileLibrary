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
        this.obj = new(obj, StringComparer.OrdinalIgnoreCase);
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

    /// <inheritdoc/>
    public TypeCode GetTypeCode() => throw new InvalidOperationException("GetTypeCode not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => throw new InvalidOperationException("ToBoolean not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => throw new InvalidOperationException("ToByte not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => throw new InvalidOperationException("ToChar not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidOperationException("ToDateTime not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => throw new InvalidOperationException("ToDecimal not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => throw new InvalidOperationException("ToDouble not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToInt16 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToInt32 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToInt64 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => throw new InvalidOperationException("ToSByte not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => throw new InvalidOperationException("ToSingle not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => throw new InvalidOperationException("ToString not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => throw new InvalidOperationException("ToType not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt16 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt32 not allowed on type of ObjectConfigOption.");

    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToUInt64 not allowed on type of ObjectConfigOption.");
}