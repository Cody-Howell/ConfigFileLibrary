using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
using HowlDev.IO.Text.Parsers;
using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// This config file takes in a file path and reads it as either a JSON, YAML, or TXT file.
/// </summary>
public class TextConfigFile : IBaseConfigOption {
    private IBaseConfigOption option;

    #region Option Exports
    /// <summary/>
    public BaseType type => option.type;
    /// <summary/>
    public int Count => option.Count;
    /// <summary/>
    public IEnumerable<IBaseConfigOption> Items => option.Items;
    /// <summary/>
    public IEnumerable<string> Keys => option.Keys;

    /// <summary/>
    public IBaseConfigOption this[string key] => option[key];
    /// <summary/>
    public IBaseConfigOption this[int index] => option[index];
    /// <summary/>
    public bool AsBool() => option.AsBool();
    /// <summary/>
    public List<bool> AsBoolList() => option.AsBoolList();
    /// <summary/>
    public double AsDouble() => option.AsDouble();
    /// <summary/>
    public List<double> AsDoubleList() => option.AsDoubleList();
    /// <summary/>
    public int AsInt() => option.AsInt();
    /// <summary/>
    public List<int> AsIntList() => option.AsIntList();
    /// <summary/>
    public string AsString() => option.AsString();
    /// <summary/>
    public List<string> AsStringList() => option.AsStringList();

    /// <summary/>
    public bool TryGet(string key, out IBaseConfigOption value) => option.TryGet(key, out value);
    /// <summary/>
    public bool Contains(string key) => option.Contains(key);
    /// <inheritdoc/>
    public TypeCode GetTypeCode() => option.GetTypeCode();
    /// <inheritdoc/>
    public bool ToBoolean(IFormatProvider? provider) => option.ToBoolean(provider);
    /// <inheritdoc/>
    public byte ToByte(IFormatProvider? provider) => option.ToByte(provider);
    /// <inheritdoc/>
    public char ToChar(IFormatProvider? provider) => option.ToChar(provider);
    /// <inheritdoc/>
    public DateTime ToDateTime(IFormatProvider? provider) => option.ToDateTime(provider);
    /// <inheritdoc/>
    public decimal ToDecimal(IFormatProvider? provider) => option.ToDecimal(provider);
    /// <inheritdoc/>
    public double ToDouble(IFormatProvider? provider) => option.ToDouble(provider);
    /// <inheritdoc/>
    public short ToInt16(IFormatProvider? provider) => option.ToInt16(provider);
    /// <inheritdoc/>
    public int ToInt32(IFormatProvider? provider) => option.ToInt32(provider);
    /// <inheritdoc/>
    public long ToInt64(IFormatProvider? provider) => option.ToInt64(provider);
    /// <inheritdoc/>
    public sbyte ToSByte(IFormatProvider? provider) => option.ToSByte(provider);
    /// <inheritdoc/>
    public float ToSingle(IFormatProvider? provider) => option.ToSingle(provider);
    /// <inheritdoc/>
    public string ToString(IFormatProvider? provider) => option.ToString(provider);
    /// <inheritdoc/>
    public object ToType(Type conversionType, IFormatProvider? provider) => option.ToType(conversionType, provider);
    /// <inheritdoc/>
    public ushort ToUInt16(IFormatProvider? provider) => option.ToUInt16(provider);
    /// <inheritdoc/>
    public uint ToUInt32(IFormatProvider? provider) => option.ToUInt32(provider);
    /// <inheritdoc/>
    public ulong ToUInt64(IFormatProvider? provider) => option.ToUInt64(provider);
    #endregion

    private List<string> acceptedExtensions = [".txt", ".yml", ".yaml", ".json"];

    private TextConfigFile() {
        option = new PrimitiveConfigOption("");
    }

    /// <summary/>
    public TextConfigFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        if (!acceptedExtensions.Contains(extension)) {
            throw new FormatException($"Extension not recognized: {extension}");
        }

        string file = File.ReadAllText(filePath);
        switch (extension) {
            case ".txt":
                option = ParseFileAsOption(new TXTParser(file));
                return;
            case ".yaml":
            case ".yml":
                option = ParseFileAsOption(new YAMLParser(file));
                break;
            case ".json":
                option = ParseFileAsOption(new JSONParser(file));
                break;
            default: throw new Exception("Extension error should be handled above. Extension was not recognized.");
        }
    }

    /// <summary>
    /// Get a configuration file without the file system through this method. <br/>
    /// If you need an option that just reads all lines as a single primitive, use
    /// the YAML type. Otherwise, pick the type that best fits the format.
    /// </summary>
    /// <param name="fileValue">JSON string</param>
    /// <param name="type">File type to parse</param>
    public static TextConfigFile ReadTextAs(FileTypes type, string fileValue) {
        TextConfigFile file = new TextConfigFile();
        switch (type) {
            case FileTypes.TXT: file.option = ParseFileAsOption(new TXTParser(fileValue)); break;
            case FileTypes.YAML: file.option = ParseFileAsOption(new YAMLParser(fileValue)); break;
            case FileTypes.JSON: file.option = ParseFileAsOption(new JSONParser(fileValue)); break;
        }
        return file;
    }

    /// <summary>
    /// Defaults to attempt a constructor call first, and if that fails, uses a parameterless constructor and 
    /// fills in any available properties. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public T As<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true });
    }

    /// <summary>
    /// Takes in an <see cref="OptionMappingOptions"/> type through to the inner Map function. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    public T As<T>(OptionMappingOptions option) {
        return Map<T>(option);
    }

    /// <summary>
    /// Defaults to attempt a constructor call first, and if that fails, uses a parameterless constructor and 
    /// fills in any available properties. <br/>
    /// Will strictly test for the number of values in the object and the constructor and/or number of writable 
    /// properties in the passed in type. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    public T AsStrict<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, UseConstructors = true, StrictMatching = true });
    }

    /// <summary>
    /// Uses constructors to build an object. It sorts by descending length of parameters 
    /// and uses the first that the object satisfies all values.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public T AsConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true });
    }

    /// <summary>
    /// Uses constructors to build an object. 
    /// It uses Strict matching, so it only checks constructors that have the same length 
    /// as the object this contains.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    public T AsStrictConstructed<T>() {
        return Map<T>(new OptionMappingOptions() { UseConstructors = true, StrictMatching = true });
    }

    /// <summary>
    /// Uses parameters to build an object. It requires a parameterless constructor to be available 
    /// on the type. 
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public T AsProperties<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true });
    }

    /// <summary>
    /// Uses parameters to build an object. It requires a parameterless constructor to be available 
    /// on the type. <br/>
    /// For Strict mapping, it will either throw an exception if the writable keys and the object count 
    /// have a different number, or will throw any property name that does not have an object equivalent. <br/><br/>
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="StrictMappingException"/>
    public T AsStrictProperties<T>() {
        return Map<T>(new OptionMappingOptions() { UseProperties = true, StrictMatching = true });
    }

    private T Map<T>(OptionMappingOptions options, IBaseConfigOption? option = null) {
        option ??= this.option; // nice

        if (options.UseConstructors) {
            var ctors = typeof(T).GetConstructors();

            if (options.UseProperties) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length > 0)];
            }

            if (options.StrictMatching) {
                ctors = [.. ctors.Where(p => p.GetParameters().Length == option.Count)];
            }

            foreach (var ctor in ctors.OrderByDescending(c => c.GetParameters().Length)) {
                var parameters = ctor.GetParameters();
                bool canCreate = parameters.All(p => Contains(p.Name!));

                if (canCreate) {
                    var args = parameters
                        .Select(p => Convert.ChangeType(option[p.Name!], p.ParameterType))
                        .ToArray();

                    return (T)ctor.Invoke(args);
                }
            }

            if (options.StrictMatching && !options.UseProperties) {
                throw new StrictMappingException(
                    $"""
                    No suitable constructor found for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: {String.Join(", ", option.Keys.ToArray())}.
                    """
                );
            }

            if (!options.UseProperties) {
                throw new InvalidOperationException(
                    $"""
                    No suitable constructor found for {typeof(T).Name}. 
                    Tried to find a constructor that matched the following keys: {String.Join(", ", option.Keys.ToArray())}.
                    """
                );
            }
        }

        if (options.UseProperties) {
            if (typeof(T).GetConstructor(Type.EmptyTypes) == null) {
                throw new InvalidOperationException(
                    $"Type {typeof(T).Name} must have a parameterless constructor to use property mapping."
                );
            }

            T instance = Activator.CreateInstance<T>()!;
            var properties = typeof(T).GetProperties().Where(p => p.CanWrite);

            if (options.StrictMatching) {
                if (properties.Count() != option.Count) {
                    throw new StrictMappingException(
                        $"""
                        Property count mismatch for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                        Property count of type: {properties.Count()}. Key count of object: {option.Count}.
                        """
                    );
                }
            }

            foreach (var prop in properties) {
                if (option.TryGet(prop.Name, out var value)) {
                    prop.SetValue(instance, Convert.ChangeType(value, prop.PropertyType));
                } else if (options.StrictMatching) {
                    throw new StrictMappingException(
                        $"""
                        Property mismatch for {typeof(T).Name}. Consider removing the StrictMatching flag. 
                        Could not find matching object key for property: {prop.Name}.
                        """
                    );
                }
            }

            return instance;
        }

        throw new InvalidOperationException(
            $"Was not able to construct object for {typeof(T).Name}."
        );
    }

    private static IBaseConfigOption ParseFileAsOption(TokenParser func) {
        var stack = new Stack<Frame>();
        stack.Push(new Frame(FrameKind.Root));

        foreach (var (type, value) in func) {
            var frame = stack.Peek();
            switch (type) {
                case TextToken.StartObject:
                    stack.Push(new Frame(FrameKind.Object));
                    break;
                case TextToken.EndObject:
                    var obj = stack.Pop();
                    var parent = stack.Peek();
                    parent.Add(obj.AsOption());
                    break;
                case TextToken.StartArray:
                    stack.Push(new Frame(FrameKind.Array));
                    break;
                case TextToken.EndArray:
                    var arr = stack.Pop();
                    parent = stack.Peek();
                    parent.Add(arr.AsOption());
                    break;
                case TextToken.KeyValue:
                    frame.PendingKey = value;
                    break;
                case TextToken.Primitive:
                    frame.Add(new PrimitiveConfigOption(value));
                    break;
                case TextToken.Comment:
                    break;
            }
        }

        Frame root = stack.Pop();
        return root.AsOption();
    }
}