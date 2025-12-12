using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseObjectOption {
    /// <summary/>
    IBaseConfigOption this[string key] { get; }
    /// <summary/>
    IEnumerable<string> Keys { get; }
    /// <summary/>
    bool TryGet(string key, out IBaseConfigOption value);
    /// <summary/>
    bool Contains(string key);
}
