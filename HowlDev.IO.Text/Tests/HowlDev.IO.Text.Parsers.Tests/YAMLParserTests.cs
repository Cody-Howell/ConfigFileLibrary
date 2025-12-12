using HowlDev.IO.Text.Parsers.Enums;
namespace HowlDev.IO.Text.Parsers.Tests;

public class YAMLParserTests {
    [Test]
    public async Task SimpleTest1() {
        string yml = """
        key: values
        """;
        List<(TextToken token, string value)> values = new(new YAMLParser(yml));
        await Assert.That(values.Count).IsEqualTo(4);

        await Assert.That(values[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[2].value).IsEqualTo("values");
        await Assert.That(values[3].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task SimpleTest2() {
        string yml = """
        key1: value1
        key2: value2
        key3: value3
        """;
        List<(TextToken token, string value)> values = new(new YAMLParser(yml));
        await Assert.That(values[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[5].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[7].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task SimpleTest3() {
        string yml = """
          - one
          - two
          - three
        """;
        List<(TextToken token, string value)> values = new(new YAMLParser(yml));
        await Assert.That(values[0].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(values[1].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[1].value).IsEqualTo("one");
        await Assert.That(values[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[4].token).IsEqualTo(TextToken.EndArray);
    }

    [Test]
    public async Task SimpleTest4() {
        string yml = """
        obj:
          list:
            - a
            - b
            - c
        """;
        List<(TextToken token, string value)> values = [.. new YAMLParser(yml)];
        await Assert.That(values[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[1].value).IsEqualTo("obj");
        await Assert.That(values[2].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[3].value).IsEqualTo("list");
        await Assert.That(values[4].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(values[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[7].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[8].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(values[9].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[10].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task SimpleTest5() {
        string yml = File.ReadAllText("../../../../HowlDev.IO.Text.ConfigFile.Tests/data/YAML/Realistic/ComplexObject.yaml");
        List<(TextToken token, string value)> values = new(new YAMLParser(yml));
        await Assert.That(values[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[2].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[4].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(values[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[7].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[8].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(values[9].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[10].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[11].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[12].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[13].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[14].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[15].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[16].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[17].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[17].value).IsEqualTo("second");
        await Assert.That(values[18].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[19].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[19].value).IsEqualTo("arrayOfObjects");
        await Assert.That(values[20].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(values[21].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[22].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[22].value).IsEqualTo("lorem");
        await Assert.That(values[23].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[24].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[25].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[26].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[27].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(values[28].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[29].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[30].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[31].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[32].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[33].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(values[34].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(values[35].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(values[36].token).IsEqualTo(TextToken.EndObject);
    }
}
