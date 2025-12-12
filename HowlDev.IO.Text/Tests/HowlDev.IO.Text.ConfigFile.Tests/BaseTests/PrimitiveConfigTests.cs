using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class PrimitiveConfigTests {
    [Test]
    [Arguments("Lorem", "Lorem")]
    [Arguments("Lorem Ipsum with a space", "Lorem Ipsum with a space")]
    [Arguments("               Lorem Ipsum with more spaces    ", "Lorem Ipsum with more spaces")]
    public async Task ReturnsString(string s, string exp) {
        PrimitiveConfigOption p = new PrimitiveConfigOption(s);
        await Assert.That(p.AsString()).IsEqualTo(exp);
    }

    [Test]
    [Arguments("15", 15)]
    [Arguments("1293875", 1293875)]
    [Arguments("-234", -234)]
    public async Task AsValidInt(string s, int exp) {
        PrimitiveConfigOption p = new PrimitiveConfigOption(s);
        await Assert.That(p.AsInt()).IsEqualTo(exp);
    }

    [Test]
    public async Task AsInvalidInts() {
        PrimitiveConfigOption p = new PrimitiveConfigOption("Lorem");
        await Assert.That(p.AsInt)
            .Throws<InvalidCastException>()
            .WithMessage($"Value \"Lorem\" is not castable to an Int.");
    }

    [Test]
    [Arguments("12.325", 12.325)]
    [Arguments("0.0001", 0.0001)]
    [Arguments("-234.345", -234.345)]
    public async Task AsValidDouble(string s, double exp) {
        PrimitiveConfigOption p = new PrimitiveConfigOption(s);
        await Assert.That(p.AsDouble()).IsEqualTo(exp);
    }

    [Test]
    public async Task AsInvalidDoubles() {
        PrimitiveConfigOption p = new PrimitiveConfigOption("34.45.56");
        await Assert.That(p.AsDouble)
            .Throws<InvalidCastException>()
            .WithMessage($"Value \"34.45.56\" is not castable to a Double.");
    }

    [Test]
    [Arguments("True", true)]
    [Arguments("true", true)]
    [Arguments("False", false)]
    [Arguments("false", false)]
    public async Task AsValidBools(string s, bool exp) {
        PrimitiveConfigOption p = new PrimitiveConfigOption(s);
        await Assert.That(p.AsBool()).IsEqualTo(exp);
    }

    [Test]
    public async Task AsInvalidBool() {
        PrimitiveConfigOption p = new PrimitiveConfigOption("tru");
        await Assert.That(p.AsBool)
            .Throws<InvalidCastException>()
            .WithMessage($"Value \"tru\" is not castable to a Boolean.");
    }

    [Test]
    public async Task InvalidIndexers() {
        PrimitiveConfigOption p = new PrimitiveConfigOption("Lorem");
        await Assert.That(() => p["Lorem"])
            .Throws<InvalidOperationException>()
            .WithMessage("Key indexing operation invalid on type of PrimitiveConfigOption.");
        await Assert.That(() => p[15])
            .Throws<InvalidOperationException>()
            .WithMessage("List indexing operation invalid on type of PrimitiveConfigOption.");
    }
}