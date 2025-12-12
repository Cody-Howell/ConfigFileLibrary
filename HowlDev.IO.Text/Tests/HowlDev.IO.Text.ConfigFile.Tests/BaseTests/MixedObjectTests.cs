using HowlDev.IO.Text.ConfigFile.Interfaces;
using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class MixedObjectTests {
    [Test]
    public async Task ObjectSecondOrderMixedTest() {
        ObjectConfigOption obj = new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
            { "first", new ArrayConfigOption(new List<IBaseConfigOption> {
                new ObjectConfigOption(new Dictionary<string, IBaseConfigOption> {
                    { "first", new PrimitiveConfigOption("10") }
                }, "test", "first"),
                new PrimitiveConfigOption("20.2"),
                new PrimitiveConfigOption("Lorem")
            }, "test") },
            { "second", new PrimitiveConfigOption("true") }
        }, "test");

        await Assert.That(obj["first"][0]["first"].AsInt()).IsEqualTo(10);
        await Assert.That(obj["first"][1].AsDouble()).IsEqualTo(20.2);
        await Assert.That(obj["first"][2].AsString()).IsEqualTo("Lorem");
        await Assert.That(obj["second"].AsBool()).IsEqualTo(true);

        await Assert.That(() => obj["first"][1].AsInt())
            .Throws<InvalidCastException>()
            .WithMessage("Value \"20.2\" is not castable to an Int.");
    }
}