namespace ConfigFileLibrary;

public class ArrayConfigOption : IBaseConfigOption {
    private List<IBaseConfigOption> array = new List<IBaseConfigOption>();
    private string resourcePath;

    public ArrayConfigOption(List<IBaseConfigOption> array, string parentPath = "", string myPath = "") {
        this.array = array;
        resourcePath = parentPath;
        if (myPath.Length > 0) resourcePath += "[" + myPath + "]";
    }

    public IBaseConfigOption this[string key] => throw new InvalidOperationException("Operation invalid on type of ArrayConfigOption.");

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

    public string AsString() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    public int AsInt() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    public double AsDouble() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");
    public bool AsBool() => throw new InvalidOperationException("Type casting not allowed on type ArrayConfigOption");

    public List<string> AsStringList() {
        List<string> outList = new List<string>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsString());
        }
        return outList;
    }

    public List<int> AsIntList() {
        List<int> outList = new List<int>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsInt());
        }
        return outList;
    }

    public List<double> AsDoubleList() {
        List<double> outList = new List<double>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsDouble());
        }
        return outList;
    }

    public List<bool> AsBoolList() {
        List<bool> outList = new List<bool>();
        foreach (IBaseConfigOption option in array) {
            outList.Add(option.AsBool());
        }
        return outList;
    }
}
