namespace ConfigFileLibrary.Enums;

#pragma warning disable 1591
public enum FileToken {
    StartObject,
    EndObject,
    StartArray,
    EndArray,
    KeyValue,
    Primitive,
    Comment
}