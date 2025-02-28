using ConfigFileLibrary;
namespace ConfigFileLibrary.Tests.YAMLFile;

public class YAMLFileTests {
    [Test]
    public async Task PrimitiveTest1() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../YAML/FirstOrder/Primitive1.yaml");
        await Assert.That(reader.AsString()).IsEqualTo("45");
        await Assert.That(reader.AsInt()).IsEqualTo(45);
    }

    [Test]
    public async Task PrimitiveTest2() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../YAML/FirstOrder/Primitive2.yaml");
        await Assert.That(reader.AsString()).IsEqualTo("test string");
    }

    [Test]
    public async Task ObjectTest() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../YAML/FirstOrder/Object.yaml");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Test String");
        await Assert.That(reader["Lorem2"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Lorem3"].AsDouble()).IsEqualTo(3.15);
        await Assert.That(reader["Lorem4"].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task BrokenObjectThrowsError() {
        await Assert.That(() => new YAMLConfigFile("../../../YAML/FirstOrder/BrokenObject.yaml"))
            .Throws<FormatException>()
            .WithMessage("Don't include multiple (:) on the same line. I read key: \"Broken\"");
    }

    [Test]
    public async Task ArrayTest() {
        YAMLConfigFile reader = new YAMLConfigFile("../../../YAML/FirstOrder/Array.yaml");
        await Assert.That(reader[0].AsString()).IsEqualTo("Test String");
        await Assert.That(reader[1].AsInt()).IsEqualTo(15);
        await Assert.That(reader[2].AsDouble()).IsEqualTo(3.15);
        await Assert.That(reader[3].AsBool()).IsEqualTo(true);
    }
}
