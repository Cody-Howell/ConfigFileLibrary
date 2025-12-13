using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class JSONParserTests {
    [Test]
    public async Task Array() {
        List<(TextToken token, string value)> parsed = new(new JSONParser(File.ReadAllText("../../../data/JSON/SimpleArray.json")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.EndArray);
    }

    [Test]
    public async Task Object() {
        List<(TextToken token, string value)> parsed = new(new JSONParser(File.ReadAllText("../../../data/JSON/SimpleObject.json")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[1].value).IsEqualTo("lorem");
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].value).IsEqualTo("this is a string");
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task ArrayWithObject() {
        List<(TextToken token, string value)> parsed = new(new JSONParser(File.ReadAllText("../../../data/JSON/ArrayWithObject.json")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[10].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[11].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[12].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[13].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[14].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[15].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[16].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[17].token).IsEqualTo(TextToken.EndArray);
    }

    [Test]
    public async Task ComplexObject() {
        List<(TextToken token, string value)> parsed = new(new JSONParser(File.ReadAllText("../../../data/JSON/ComplexObject.json")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[7].value).IsEqualTo("3");
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[10].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[11].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[11].value).IsEqualTo("other sibling");
        await Assert.That(parsed[12].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[13].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[14].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[15].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[16].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[17].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[17].value).IsEqualTo("second");
        await Assert.That(parsed[18].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[19].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[19].value).IsEqualTo("arrayOfObjects");
        await Assert.That(parsed[20].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[21].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[22].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[23].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[24].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[24].value).IsEqualTo("something");
        await Assert.That(parsed[25].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[25].value).IsEqualTo("1.2");
        await Assert.That(parsed[26].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[27].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[28].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[29].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[30].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[31].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[31].value).IsEqualTo("false");
        await Assert.That(parsed[32].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[33].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[34].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[35].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[35].value).IsEqualTo("hopefully");
        await Assert.That(parsed[36].token).IsEqualTo(TextToken.EndObject);
        await Assert.That(parsed[37].token).IsEqualTo(TextToken.EndObject);
    }

    [Test]
    public async Task ObjectWithArray() {
        List<(TextToken token, string value)> parsed = new(new JSONParser(File.ReadAllText("../../../data/JSON/ObjectWithArray.json")));
        await Assert.That(parsed[0].token).IsEqualTo(TextToken.StartObject);
        await Assert.That(parsed[1].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[2].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[3].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[4].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[5].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[6].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[7].token).IsEqualTo(TextToken.KeyValue);
        await Assert.That(parsed[8].token).IsEqualTo(TextToken.StartArray);
        await Assert.That(parsed[9].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[10].token).IsEqualTo(TextToken.Primitive);
        await Assert.That(parsed[11].token).IsEqualTo(TextToken.EndArray);
        await Assert.That(parsed[12].token).IsEqualTo(TextToken.EndObject);
    }
}