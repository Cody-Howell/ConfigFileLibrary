using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseArrayOption : IBasePrimitiveOption {
    /// <summary/>
    IBaseConfigOption this[int index] { get; }

    /// <summary/>
    List<string> AsStringList();
    /// <summary/>
    List<int> AsIntList();
    /// <summary/>
    List<double> AsDoubleList();
    /// <summary/>
    List<bool> AsBoolList();
}
