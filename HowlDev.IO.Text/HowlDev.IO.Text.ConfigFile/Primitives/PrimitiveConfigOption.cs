using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Primitives;

/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public class PrimitiveConfigOption : IBaseConfigOption {
    private string value;

    /// <summary/>
    public BaseType type => BaseType.Primitive;

    /// <summary/>
    public IEnumerable<string> Keys => throw new InvalidOperationException("Key enumeration not allowed on type of PrimitiveConfigOption.");
    /// <summary/>
    public int Count => throw new InvalidOperationException("Count not allowed on type of PrimitiveConfigOption.");
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => throw new InvalidOperationException("Item enumeration not allowed on type of PrimitiveConfigOption.");

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

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) {
        throw new InvalidOperationException("TryGet not allowed on type PrimitiveConfigOption");
    }

    /// <summary/>
    public bool Contains(string key) {
        throw new InvalidOperationException("Contains not allowed on type PrimitiveConfigOption");
    }

    /// <inheritdoc/>
    public TypeCode GetTypeCode() => TypeCode.String;

    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => AsBool();

    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => throw new InvalidOperationException("ToByte not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => throw new InvalidOperationException("ToChar not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidOperationException("ToDateTime not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => (decimal)AsDouble();

    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => AsDouble();

    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => (short)AsInt();

    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => AsInt();

    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => throw new InvalidOperationException("ToInt64 not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => throw new InvalidOperationException("ToSByte not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => throw new InvalidOperationException("ToSingle not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => AsString();

    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => throw new InvalidOperationException("ToType not supported on type PrimitiveConfigOption");

    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => (ushort)AsInt();

    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => (uint)AsInt();

    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => (ulong)AsInt();
}