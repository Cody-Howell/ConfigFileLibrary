using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseObjectOption : IBasePrimitiveOption, IBaseArrayOption {
    /// <summary/>
    IBaseConfigOption this[string key] { get; }

}
