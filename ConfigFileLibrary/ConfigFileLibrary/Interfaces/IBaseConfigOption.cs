using ConfigFileLibrary.Enums;
using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseConfigOption : IBasePrimitiveOption, IBaseArrayOption, IBaseObjectOption {
    /// </summary>
    BaseType type { get; }
}
