using System.Reflection.PortableExecutable;

namespace ConfigFileLibrary.Tests;

public class SingleLineTests {
    [Test]
    public async Task StringValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt"); // Path from my test area to my test file
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("Simple Output");
    }

    [Test]
    public async Task IntValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/Int.txt"); 
        await Assert.That(reader.AsInt("Lorem")).IsEqualTo(15);
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("15");
    }

    [Test]
    public async Task IntThrowsErrorWithHelpfulMessage() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt");
        await Assert.That(() => reader.AsInt("Lorem"))
            .Throws<NotSupportedException>()
            .WithMessage("This string (line 1) can't be parsed to an Int.");
    }

    [Test]
    public async Task DoubleValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/Double.txt");
        await Assert.That(reader.AsDouble("Lorem")).IsEqualTo(42.5);
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("42.5");
    }

    [Test]
    public async Task DoubleThrowsErrorWithHelpfulMessage() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt");
        await Assert.That(() => reader.AsDouble("Lorem"))
            .Throws<NotSupportedException>()
            .WithMessage("This string (line 1) can't be parsed to a Double.");
    }

    [Test]
    public async Task BoolValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/Bool.txt");
        await Assert.That(reader.AsBool("Lorem")).IsEqualTo(true);
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("true");
    }

    [Test]
    public async Task BoolThrowsErrorWithHelpfulMessage() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt");
        await Assert.That(() => reader.AsBool("Lorem"))
            .Throws<NotSupportedException>()
            .WithMessage("This string (line 1) can't be parsed to a Boolean.");
    }

    [Test]
    public async Task NoLineSplitterThrowsError() {
        await Assert.That(() => new TXTConfigFile("../../../TXT/1LineTests/NoSplitter.txt"))
            .Throws<Exception>()
            .WithMessage("No split character was found at line 1. Please add it or override your custom split character.");
    }

    [Test]
    public async Task TwoLineSplitterThrowsError() {
        await Assert.That(() => new TXTConfigFile("../../../TXT/1LineTests/TwoSplitters.txt"))
            .Throws<Exception>()
            .WithMessage("More than 1 split character was found at line 1. Please remove it or change your character to something else.");
    }

    [Test]
    public async Task CustomSplitterWorksCorrectly() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/CustomSplitter.txt", ' ');
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("Testing");
    }

    [Test]
    public async Task StringsAreTrimmedBeforeUse() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/ManySpaces.txt");
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("Fourteen spaces probably");
    }

    [Test]
    public async Task IntsAreTrimmedBeforeUse() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/ManySpacesInt.txt");
        await Assert.That(reader.AsInt("Lorem")).IsEqualTo(22);
    }

    [Test]
    public async Task RetreivingInvalidKeyThrowsError() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt");
        await Assert.That(() => reader.AsString("Random Thing"))
            .Throws<KeyNotFoundException>()
            .WithMessage("Key (\"Random Thing\") was not found in the file.");
    }

    [Test]
    public async Task KeysAreProperlyRetreived() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/1LineTests/String.txt");
        await Assert.That(reader.Keys).IsEquivalentTo(["Lorem"]);
    }
}
public class FiveLineTests {
    [Test]
    public async Task FiveStringTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/AllStrings.txt");
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("String 1");
        await Assert.That(reader.AsString("Lorem2")).IsEqualTo("String 2");
        await Assert.That(reader.AsString("Lorem3")).IsEqualTo("String 3");
        await Assert.That(reader.AsString("Lorem4")).IsEqualTo("String 4");
        await Assert.That(reader.AsString("Lorem5")).IsEqualTo("String 5");
    }

    [Test]
    public async Task FiveIntTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/AllInts.txt");
        await Assert.That(reader.AsInt("Lorem")).IsEqualTo(1);
        await Assert.That(reader.AsInt("Lorem2")).IsEqualTo(2);
        await Assert.That(reader.AsInt("Lorem3")).IsEqualTo(-3);
        await Assert.That(reader.AsInt("Lorem4")).IsEqualTo(4);
        await Assert.That(reader.AsInt("Lorem5")).IsEqualTo(5);
    }

    [Test]
    public async Task FiveDoubleTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/AllDoubles.txt");
        await Assert.That(reader.AsDouble("Lorem")).IsEqualTo(1.23);
        await Assert.That(reader.AsDouble("Lorem2")).IsEqualTo(2.23);
        await Assert.That(reader.AsDouble("Lorem3")).IsEqualTo(-3.23);
        await Assert.That(reader.AsDouble("Lorem4")).IsEqualTo(4.23);
        await Assert.That(reader.AsDouble("Lorem5")).IsEqualTo(5.23);
    }

    [Test]
    public async Task FiveBoolsTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/AllBools.txt");
        await Assert.That(reader.AsBool("Lorem")).IsEqualTo(true);
        await Assert.That(reader.AsBool("Lorem2")).IsEqualTo(false);
        await Assert.That(reader.AsBool("Lorem3")).IsEqualTo(true);
        await Assert.That(reader.AsBool("Lorem4")).IsEqualTo(false);
        await Assert.That(reader.AsBool("Lorem5")).IsEqualTo(true);
    }

    [Test]
    public async Task ValidKeysAreDisplayed() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/AllStrings.txt");
        await Assert.That(reader.Keys).IsEquivalentTo([
            "Lorem",
            "Lorem2",
            "Lorem3",
            "Lorem4",
            "Lorem5",
            ]);
    }

    [Test]
    public async Task MixedValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/5LineTests/Mixed.txt");
        await Assert.That(reader.AsString("Lorem")).IsEqualTo("Test string");
        await Assert.That(reader.AsInt("Lorem2")).IsEqualTo(15);
        await Assert.That(reader.AsDouble("Lorem3")).IsEqualTo(3.25);
        await Assert.That(reader.AsBool("Lorem4")).IsEqualTo(true);
        await Assert.That(reader.AsInt("Lorem5")).IsEqualTo(-25);
    }

    [Test]
    public async Task ErrorLinesDisplayCorrectly() {
        await Assert.That(() => new TXTConfigFile("../../../TXT/5LineTests/Error.txt"))
            .Throws<FormatException>()
            .WithMessage("More than 1 split character was found at line 4. Please remove it or change your character to something else.");
    }
}
public class RealisticTests {
    [Test]
    public async Task FiveStringTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../TXT/Realistic/File1.txt");
        await Assert.That(reader.AsString("Enemy Name")).IsEqualTo("Bad Guy");
        await Assert.That(reader.AsString("Enemy Color")).IsEqualTo("#9645ff");
        await Assert.That(reader.AsBool("Is Boss")).IsEqualTo(true);
        await Assert.That(reader.AsInt("Enemy Speed")).IsEqualTo(15);
        await Assert.That(reader.AsDouble("Enemy Damage")).IsEqualTo(23.4);
    }
}
