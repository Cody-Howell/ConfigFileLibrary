using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile;

/// <summary>
/// A mismatch of parameters in an <see cref="ObjectConfigOption"/>, either over or under, 
/// thrown in an As____&lt;T&gt;() function call.
/// </summary>
public class StrictMappingException : Exception;