using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseConfigOption {
    /// <summary/>
    IBaseConfigOption this[string key] { get; }
    /// <summary/>
    IBaseConfigOption this[int index] { get; }
    /// <summary/>
    string AsString();
    /// <summary/>
    int AsInt();
    /// <summary/>
    double AsDouble();
    /// <summary/>
    bool AsBool();

    /// <summary/>
    List<string> AsStringList();
    /// <summary/>
    List<int> AsIntList();
    /// <summary/>
    List<double> AsDoubleList();
    /// <summary/>
    List<bool> AsBoolList();
}