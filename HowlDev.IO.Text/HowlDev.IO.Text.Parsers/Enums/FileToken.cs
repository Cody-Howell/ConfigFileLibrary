namespace HowlDev.IO.Text.Parsers.Enums;

#pragma warning disable 1591
public enum TextToken {
    StartObject,
    EndObject,
    StartArray,
    EndArray,
    KeyValue,
    Primitive,
    Comment
}