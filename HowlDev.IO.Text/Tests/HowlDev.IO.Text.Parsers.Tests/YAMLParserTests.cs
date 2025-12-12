using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class YAMLParserTests {
    [Test]
    public async Task Primitive2() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Primitive2.yaml")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[0].value).IsEqualTo("This is a sample multiline string that may become useful");
    }

    [Test]
    public async Task Array() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Array.yaml")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.EndArray);
    }

    [Test]
    public async Task Object() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Object.yaml")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].value).IsEqualTo("3.15");
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task ArrayWithObject() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/ArrayWithObject.yaml")));
    }

    [Test]
    public async Task ComplexObject() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/ComplexObject.yaml")));
    }

    [Test]
    public async Task MixedArray() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/MixedArray.yml")));
    }

    [Test]
    public async Task ObjectWithArray() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/ObjectWithArray.yaml")));
    }
}