using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile;

internal class Frame {
    public FrameKind Kind { get; }
    private Dictionary<string, IBaseConfigOption>? Obj;
    private List<IBaseConfigOption>? Arr;
    private IBaseConfigOption? option = null;


    private string? pendingKey;
    public string? PendingKey {
        get { return pendingKey; }
        set {
            if (Kind == FrameKind.Array) throw new Exception("Array kind cannot have pending key.");
            pendingKey = value;
        }
    }

    public Frame(FrameKind kind) {
        Kind = kind;
        if (kind == FrameKind.Object) Obj = [];
        if (kind == FrameKind.Array) Arr = [];
    }

    public IBaseConfigOption AsOption() {
        if (Kind == FrameKind.Root) return option!;
        if (Kind == FrameKind.Object) return new ObjectConfigOption(Obj!);
        return new ArrayConfigOption(Arr!);
    }

    public void Add(IBaseConfigOption option) {
        if (Kind == FrameKind.Root) {
            if (this.option is not null) throw new Exception("Root cannot have two base objects.");
            this.option = option;
        } else if (Kind == FrameKind.Object) {
            if (PendingKey is null) throw new Exception("Object must provide a pending key.");
            Obj!.Add(PendingKey, option);
        } else { // Type is Array
            Arr!.Add(option);
        }
    }
}

enum FrameKind { Object, Array, Root }