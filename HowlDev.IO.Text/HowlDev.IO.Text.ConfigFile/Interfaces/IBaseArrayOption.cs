using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Interfaces;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseArrayOption : IBasePrimitiveOption {
    /// <summary/>
    IBaseConfigOption this[int index] { get; }
    /// <summary/>
    int Count { get; }
    /// <summary/>
    IEnumerable<IBaseConfigOption> Items { get; }

    /// <summary/>
    List<string> AsStringList();
    /// <summary/>
    List<int> AsIntList();
    /// <summary/>
    List<double> AsDoubleList();
    /// <summary/>
    List<bool> AsBoolList();
}