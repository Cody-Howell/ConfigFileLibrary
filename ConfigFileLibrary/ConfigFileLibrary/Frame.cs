using System.ComponentModel;
using ConfigFileLibrary.Primitives;

internal class Frame {
    public FrameKind Kind { get; }
    public Dictionary<string, IBaseConfigOption>? Obj;
    public List<IBaseConfigOption>? Arr;

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
        if (Kind == FrameKind.Object) return new ObjectConfigOption(Obj!);
        return new ArrayConfigOption(Arr!);
    }

    public void Add(IBaseConfigOption option) {
        if (Kind == FrameKind.Object) {
            if (PendingKey is null) throw new Exception("Object must provide a pending key.");
            Obj.Add(PendingKey, option);
        } else { // Type is Array
            Arr.Add(option);
        }
    }
}

enum FrameKind { Object, Array }