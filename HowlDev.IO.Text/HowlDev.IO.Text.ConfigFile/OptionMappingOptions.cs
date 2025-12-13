namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// Configure the As___&lt;T&gt;() functions. You can allow constructors, properties, 
/// and strict matches. All default to False. See those comments for more details. 
/// </summary>
public record OptionMappingOptions
{
    /// <summary>
    /// Uses available constructors to construct an object. It sorts by descending length of parameters 
    /// and uses the first that the object satisfies all values. <br/>
    /// Does nothing inside the AsProperties&lt;T&gt;() function.
    /// </summary>
    public bool UseConstructors { get; init; } = false;
    /// <summary>
    /// Uses externally writable properties to fill parameters inside of an object. <br/>
    /// Does nothing inside the AsConstructed&lt;T&gt;() function.
    /// </summary>
    public bool UseProperties { get; init; } = false;
    /// <summary>
    /// Enforces an exact match for property and/or constructor fields (whichever it's currently checking). Throws an error if the 
    /// length of the option is different from an available constructor or from all writable properties. <br/>
    /// Works in both the AsProperties&lt;T&gt;() and AsConstructed&lt;T&gt;() functions.
    /// </summary>
    public bool StrictMatching { get; init; } = false;
}