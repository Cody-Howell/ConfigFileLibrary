using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class YAMLParserTests {
    [Test]
    public async Task Primitive2() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Primitive2.yaml")));
    }

    [Test]
    public async Task Array() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Array.yaml")));
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
    public async Task Object() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/Object.yaml")));
    }

    [Test]
    public async Task ObjectWithArray() {
        List<(TextToken token, string value)> parsed = new(new YAMLParser(File.ReadAllText("../../../data/YAML/ObjectWithArray.yaml")));
    }
}