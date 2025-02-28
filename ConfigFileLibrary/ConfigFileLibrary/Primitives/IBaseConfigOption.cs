using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseConfigOption {
    IBaseConfigOption this[string key] { get; }
    IBaseConfigOption this[int index] { get; }
    string AsString();
    int AsInt();
    double AsDouble();
    bool AsBool();

    List<string> AsStringList();
    List<int> AsIntList();
    List<double> AsDoubleList();
    List<bool> AsBoolList();
}
