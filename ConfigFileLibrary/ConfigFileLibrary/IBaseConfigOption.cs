namespace ConfigFileLibrary;

public interface IBaseConfigOption {
    int LineNumber { get; }
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
