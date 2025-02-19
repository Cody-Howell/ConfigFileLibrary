namespace ConfigFileLibrary.Tests.Object;

public class ObjectConfigTests {
    [Test]
    public async Task SingleObjectWorks() {
        var config = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "key", new PrimitiveConfigOption("value") }
        });
        await Assert.That(config["key"].AsString()).IsEqualTo("value");
    }

    [Test]
    public async Task ObjectHoldsMultipleKeys() {
        var config = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "key1", new PrimitiveConfigOption("value1") },
            { "key2", new PrimitiveConfigOption("value2") }
        });
        await Assert.That(config["key1"].AsString()).IsEqualTo("value1");
        await Assert.That(config["key2"].AsString()).IsEqualTo("value2");
    }

    [Test]
    public async Task ObjectThrowsCorrectError() {
        var config = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "key1", new PrimitiveConfigOption("value1") }
        });
        await Assert.That(() => config["key2"])
            .Throws<KeyNotFoundException>()
            .WithMessage("File did not contain key \"key2\".");
    }
}
