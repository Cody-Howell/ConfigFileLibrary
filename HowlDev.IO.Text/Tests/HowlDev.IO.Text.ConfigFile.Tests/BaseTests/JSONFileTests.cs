using HowlDev.IO.Text.ConfigFile.Enums;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderJSONFileTests {
    [Test]
    public async Task ArrayTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/SimpleArray.json");

        await Assert.That(reader[0].AsInt()).IsEqualTo(14);
        await Assert.That(reader[1].AsString()).IsEqualTo("lorem string");
        await Assert.That(reader[2].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task ObjectTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/SimpleObject.json");

        await Assert.That(reader["lorem"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["works"].AsDouble()).IsEqualTo(2.5);
        await Assert.That(reader["string"].AsString()).IsEqualTo("this is a string");
    }

    [Test]
    public async Task IntArrayTest() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/FirstOrder/IntArray.json");

        List<int> ints = reader.AsIntList();

        await Assert.That(ints.Count).IsEqualTo(4);
        await Assert.That(ints[0]).IsEqualTo(1);
        await Assert.That(ints[1]).IsEqualTo(3);
        await Assert.That(ints[2]).IsEqualTo(6);
        await Assert.That(ints[3]).IsEqualTo(7);
    }
}
public class SecondOrderJSONFileTests {
    [Test]
    public async Task ArrayWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ArrayWithArray.json");

        await Assert.That(reader[0][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader[0][1].AsBool()).IsEqualTo(true);
        await Assert.That(reader[0][2].AsString()).IsEqualTo("string");

        await Assert.That(reader[1][0].AsString()).IsEqualTo("second array");
        await Assert.That(reader[1][1].AsDouble()).IsEqualTo(5.3);
        await Assert.That(reader[1][2].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ArrayWithObject.json");

        await Assert.That(reader[0]["num"].AsInt()).IsEqualTo(1);
        await Assert.That(reader[0]["happy"].AsString()).IsEqualTo("maybe");
        await Assert.That(reader[0]["number"].AsDouble()).IsEqualTo(5.3);

        await Assert.That(reader[1]["num"].AsInt()).IsEqualTo(2);
        await Assert.That(reader[1]["happy"].AsString()).IsEqualTo("yes");
        await Assert.That(reader[1]["number"].AsBool()).IsEqualTo(false); // Because they don't have to be consistent
    }

    [Test]
    public async Task ObjectWithObject() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ObjectWithObject.json");

        await Assert.That(reader["first"]["type"].AsString()).IsEqualTo("object");
        await Assert.That(reader["first"]["number"].AsInt()).IsEqualTo(15);

        await Assert.That(reader["second"]["type"].AsString()).IsEqualTo("still an object");
        await Assert.That(reader["second"]["boolean"].AsBool()).IsEqualTo(true);
    }


    [Test]
    public async Task ObjectWithArray() {
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/SecondOrder/ObjectWithArray.json");

        await Assert.That(reader["first"][0].AsInt()).IsEqualTo(15);
        await Assert.That(reader["first"][1].AsString()).IsEqualTo("open string");
        await Assert.That(reader["first"][2].AsBool()).IsEqualTo(false);

        await Assert.That(reader["second"][0].AsDouble()).IsEqualTo(2.3);
        await Assert.That(reader["second"][1].AsString()).IsEqualTo("closed string");
    }
}
public class RealisticJSONFileTests {
    [Test]
    public async Task RealisticTest() {
        // Copied from the YAML system because I was lazy. But they're interoperable!
        TextConfigFile reader = new TextConfigFile("../../../data/JSON/Realistic/ComplexObject.json");

        await Assert.That(reader["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader["first"]["simple Array"][1].AsInt()).IsEqualTo(2);
        await Assert.That(reader["first"]["simple Array"][2].AsInt()).IsEqualTo(3);
        await Assert.That(reader["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader["second"]["arrayOfObjects"][0]["something"].AsDouble()).IsEqualTo(1.2);
        await Assert.That(reader["second"]["arrayOfObjects"][1]["lorem2"].AsString()).IsEqualTo("ipsum2");
        await Assert.That(reader["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["second"]["otherThing"].AsString()).IsEqualTo("hopefully");
    }
}

public class TextReadingJSONTests {
    [Test]
    public async Task ReadingTest1() {
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.JSON, "[ 14,  \"lorem string\",  true  ]");
        await Assert.That(reader[0].AsInt()).IsEqualTo(14);
        await Assert.That(reader[1].AsString()).IsEqualTo("lorem string");
        await Assert.That(reader[2].AsBool()).IsEqualTo(true);
    }
}