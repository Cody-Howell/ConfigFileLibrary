using ConfigFileLibrary;
namespace ConfigFileLibrary.Tests.YAMLFile;

public class FirstOrderTests {
    [Test]
    public async Task PrimitiveTest1() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/FirstOrder/Primitive1.yaml");
        await Assert.That(reader.AsString()).IsEqualTo("45");
        await Assert.That(reader.AsInt()).IsEqualTo(45);
    }

    [Test]
    public async Task PrimitiveTest2() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/FirstOrder/Primitive2.yaml");
        await Assert.That(reader.AsString()).IsEqualTo("This is a sample multiline string that may become useful");
    }

    [Test]
    public async Task ObjectTest() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/FirstOrder/Object.yaml");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Test String");
        await Assert.That(reader["Lorem2"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Lorem3"].AsDouble()).IsEqualTo(3.15);
        await Assert.That(reader["Lorem4"].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task BrokenObjectThrowsError() {
        await Assert.That(() => new YAMLConfigFile("../../../data/YAML/FirstOrder/BrokenObject.yaml"))
            .Throws<FormatException>()
            .WithMessage("Don't include multiple (:) on the same line. I read key: \"Broken\"");
    }

    [Test]
    public async Task ArrayTest() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/FirstOrder/Array.yaml");
        await Assert.That(reader[0].AsString()).IsEqualTo("Test String");
        await Assert.That(reader[1].AsInt()).IsEqualTo(15);
        await Assert.That(reader[2].AsDouble()).IsEqualTo(3.15);
        await Assert.That(reader[3].AsBool()).IsEqualTo(true);
    }
}
public class SecondOrderTests {
    [Test]
    public async Task ObjectWithObject() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/SecondOrder/ObjectWithObject.yaml");

        await Assert.That(reader["first"]["lorem"].AsString()).IsEqualTo("test");
        await Assert.That(reader["first"]["num"].AsInt()).IsEqualTo(1);
        await Assert.That(reader["first"]["double"].AsDouble()).IsEqualTo(2.0);
        await Assert.That(reader["first"]["bool"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["second"]["ipsum"].AsString()).IsEqualTo("something");
        await Assert.That(reader["second"]["num"].AsInt()).IsEqualTo(2);
        await Assert.That(reader["second"]["double"].AsDouble()).IsEqualTo(3.2);
        await Assert.That(reader["second"]["bool"].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task ObjectWithArray() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/SecondOrder/ObjectWithArray.yaml");

        await Assert.That(reader["first"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader["first"][1].AsInt()).IsEqualTo(5);
        await Assert.That(reader["first"][2].AsString()).IsEqualTo("string");
        await Assert.That(reader["second"][0].AsString()).IsEqualTo("this should have 4 spaces before it");
        await Assert.That(reader["second"][1].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayWithObject() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/SecondOrder/ArrayWithObject.yaml");

        await Assert.That(reader[0]["lorem"].AsString()).IsEqualTo("inside");
        await Assert.That(reader[0]["whatsthis"].AsDouble()).IsEqualTo(2.5);
        await Assert.That(reader[1]["arrayCount"].AsString()).IsEqualTo("second");
        await Assert.That(reader[1]["bool"].AsBool()).IsEqualTo(false);
        await Assert.That(reader[1]["int"].AsInt()).IsEqualTo(5);
    }

    [Test]
    public async Task ArrayWithArray() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/SecondOrder/ArrayWithArray.yaml");

        await Assert.That(reader[0][0].AsString()).IsEqualTo("lorem");
        await Assert.That(reader[0][1].AsDouble()).IsEqualTo(2.5);
        await Assert.That(reader[0][2].AsString()).IsEqualTo("allowed");
        await Assert.That(reader[1][0].AsString()).IsEqualTo("second");
        await Assert.That(reader[1][1].AsInt()).IsEqualTo(3);
        await Assert.That(reader[1][2].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task MixedArray() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/SecondOrder/MixedArray.yml");

        await Assert.That(reader[0]["object"].AsString()).IsEqualTo("this is");
        await Assert.That(reader[0]["part2"].AsString()).IsEqualTo("still part of this object");
        await Assert.That(reader[0]["part3"].AsDouble()).IsEqualTo(45.3);
        await Assert.That(reader[1].AsInt()).IsEqualTo(15);
        await Assert.That(reader[2].AsString()).IsEqualTo("test string");
    }
}
public class RealisticTests {
    [Test]
    public async Task RealisticTest() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../data/YAML/Realistic/ComplexObject.yaml");
        
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
