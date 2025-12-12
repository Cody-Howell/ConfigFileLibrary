using HowlDev.IO.Text.Parsers.Enums;
using System.ComponentModel;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class TXTParserTests {
    [Test]
    public async Task File1() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/File1.txt")));

    }

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
    }

    [Test]
    public async Task MixedArray() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/MixedArray.txt")));
    }

    [Test]
    public async Task FourLineArray() {
        List<(TextToken token, string value)> parsed = new(new TXTParser(File.ReadAllText("../../../data/TXT/FourLineArray.txt")));
    }
}