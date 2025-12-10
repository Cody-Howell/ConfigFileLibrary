using System.ComponentModel;

namespace ConfigFileLibrary.Primitives;
/// <summary/>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IBasePrimitiveOption {
    /// <summary/>
    string AsString();
    /// <summary/>
    int AsInt();
    /// <summary/>
    double AsDouble();
    /// <summary/>
    bool AsBool();
}
