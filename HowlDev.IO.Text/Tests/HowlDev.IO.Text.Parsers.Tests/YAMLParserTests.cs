using HowlDev.IO.Text.Parsers.Enums;
namespace HowlDev.IO.Text.Parsers.Tests;

public class YAMLParserTests
{
    [Test]
    public async Task SimpleTest1()
    {
        string yml = """
        key: values
        """;
        List<(TextToken, string)> values = new(new YAMLParser(yml));
        await Assert.That(values.Count).IsEqualTo(4);

        await Assert.That(values[0].Item1).IsEqualTo(TextToken.StartObject); 
        await Assert.That(values[1].Item1).IsEqualTo(TextToken.KeyValue); 
        await Assert.That(values[2].Item1).IsEqualTo(TextToken.Primitive); 
        await Assert.That(values[2].Item2).IsEqualTo("values"); 
        await Assert.That(values[3].Item1).IsEqualTo(TextToken.EndObject); 
    }
}
