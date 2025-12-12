using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class TXTParserTests {
    [Test]
    public async Task String() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/String.txt")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[1].value).IsEqualTo("Lorem");
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[2].value).IsEqualTo("Simple Output");
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task MixedObject() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/MixedObject.txt")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[10].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[11].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task MixedArray() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/MixedArray.txt")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[1].value).IsEqualTo("Mixed Array");
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task FourLineArray() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/FourLineArray.txt")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[1].value).IsEqualTo("Four Line Array");
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[10].token).IsEqualTo(TextToken.EndObject);
    }
}