using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderObjectConfigTests {
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
        }, "test");
        await Assert.That(() => config["key2"])
            .Throws<KeyNotFoundException>()
            .WithMessage("Object does not contain key \"key2\".\n\tPath: test");
    }

    [Test]
    public async Task ObjectDoesntAllowCaseInsensitiveKeys() {
        await Assert.That(() =>
        new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "key1", new PrimitiveConfigOption("value1") },
            { "Key1", new PrimitiveConfigOption("value1") }
        }, "test"))
            .Throws<ArgumentException>()
            .WithMessage("An item with the same key has already been added. Key: Key1");
    }

    [Test]
    public async Task ObjectCannotDuplicateKeys() {
        await Assert.That(() => new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "key1", new PrimitiveConfigOption("value1") },
            { "key1", new PrimitiveConfigOption("value2") }
        }, "test"))
            .Throws<ArgumentException>()
            .WithMessage("An item with the same key has already been added. Key: key1");
    }
}
public class SecondOrderObjectConfigTests {
    [Test]
    public async Task SimpleSecondOrderObject() {
        ObjectConfigOption c = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption>{
                { "second", new PrimitiveConfigOption("10")}
            })}
        });
        await Assert.That(c["first"]["second"].AsInt()).IsEqualTo(10);
    }

    [Test]
    public async Task SimpleSecondOrderObjectWithPrimitives() {
        ObjectConfigOption c = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption>{
                { "second", new PrimitiveConfigOption("10")}
            })},
            {"prim", new PrimitiveConfigOption("true") }
        });
        await Assert.That(c["first"]["second"].AsInt()).IsEqualTo(10);
        await Assert.That(c["prim"].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task SimpleSecondOrderObjectThrowsErrors() {
        ObjectConfigOption c = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption>{
                { "second", new PrimitiveConfigOption("10")}
            }, "test", "first")},
            {"prim", new PrimitiveConfigOption("true") }
        }, "test");
        await Assert.That(() => c["first"]["third"])
            .Throws<KeyNotFoundException>()
            .WithMessage("Object does not contain key \"third\".\n\tPath: test[first]");
    }
}
public class ThirdOrderObjectConfigTests {
    [Test]
    public async Task SimpleThirdOrderObjectWithPrimitives() {
        ObjectConfigOption c = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption>{
                { "second", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
                    { "third", new PrimitiveConfigOption("10") }
                })}
            })},
        });
        await Assert.That(c["first"]["second"]["third"].AsInt()).IsEqualTo(10);
    }

    [Test]
    public async Task SimpleThirdOrderObjectThrowsErrors() {
        ObjectConfigOption c = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption>{
                { "second", new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
                    { "third", new PrimitiveConfigOption("10") }
                }, "test[first]", "second")}
            }, "test", "first")},
        }, "test");
        await Assert.That(() => c["first"]["second"]["fourth"])
            .Throws<KeyNotFoundException>()
            .WithMessage("Object does not contain key \"fourth\".\n\tPath: test[first][second]");
    }
}