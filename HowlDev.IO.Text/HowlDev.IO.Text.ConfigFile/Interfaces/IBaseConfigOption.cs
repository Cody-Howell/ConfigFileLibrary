using HowlDev.IO.Text.ConfigFile.Enums;
using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Interfaces;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBaseConfigOption : IBasePrimitiveOption, IBaseArrayOption, IBaseObjectOption {
    /// <summary/>
    BaseType type { get; }
}
