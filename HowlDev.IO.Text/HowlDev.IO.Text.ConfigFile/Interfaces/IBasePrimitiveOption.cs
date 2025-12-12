using System.ComponentModel;

namespace HowlDev.IO.Text.ConfigFile.Interfaces;
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