namespace ConfigFileLibrary.Tests.TXTFile;

public class SingleLineTests {
    [Test]
    public async Task StringValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/String.txt"); // Path from my test area to my test file
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Simple Output");
    }

    [Test]
    public async Task IntValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/Int.txt");
        await Assert.That(reader["Lorem"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("15");
    }

    [Test]
    public async Task DoubleValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/Double.txt");
        await Assert.That(reader["Lorem"].AsDouble()).IsEqualTo(42.5);
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("42.5");
    }

    [Test]
    public async Task BoolValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/Bool.txt");
        await Assert.That(reader["Lorem"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("true");
    }

    [Test]
    public async Task NoLineSplitterThrowsError() {
        await Assert.That(() => new TXTConfigFile("../../../data/TXT/1LineTests/NoSplitter.txt"))
            .Throws<Exception>()
            .WithMessage("No split character was found at line 1. Please add it or override your custom split character.");
    }

    [Test]
    public async Task TwoLineSplitterThrowsError() {
        await Assert.That(() => new TXTConfigFile("../../../data/TXT/1LineTests/TwoSplitters.txt"))
            .Throws<Exception>()
            .WithMessage("More than 1 split character was found at line 1. Please remove it or change your character to something else.");
    }

    [Test]
    public async Task CustomSplitterWorksCorrectly() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/CustomSplitter.txt", ' ');
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Testing");
    }

    [Test]
    public async Task StringsAreTrimmedBeforeUse() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/ManySpaces.txt");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Fourteen spaces probably");
    }

    [Test]
    public async Task IntsAreTrimmedBeforeUse() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/ManySpacesInt.txt");
        await Assert.That(reader["Lorem"].AsInt()).IsEqualTo(22);
    }

    [Test]
    public async Task RetreivingInvalidKeyThrowsError() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/1LineTests/String.txt");
        await Assert.That(() => reader["Random Thing"].AsString())
            .Throws<KeyNotFoundException>()
            .WithMessage("Object does not contain key \"Random Thing\".\n\tPath: String.txt");
    }
}
public class FiveLineTests {
    [Test]
    public async Task FiveStringTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/AllStrings.txt");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("String 1");
        await Assert.That(reader["Lorem2"].AsString()).IsEqualTo("String 2");
        await Assert.That(reader["Lorem3"].AsString()).IsEqualTo("String 3");
        await Assert.That(reader["Lorem4"].AsString()).IsEqualTo("String 4");
        await Assert.That(reader["Lorem5"].AsString()).IsEqualTo("String 5");
    }

    [Test]
    public async Task FiveIntTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/AllInts.txt");
        await Assert.That(reader["Lorem"].AsInt()).IsEqualTo(1);
        await Assert.That(reader["Lorem2"].AsInt()).IsEqualTo(2);
        await Assert.That(reader["Lorem3"].AsInt()).IsEqualTo(-3);
        await Assert.That(reader["Lorem4"].AsInt()).IsEqualTo(4);
        await Assert.That(reader["Lorem5"].AsInt()).IsEqualTo(5);
    }

    [Test]
    public async Task FiveDoubleTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/AllDoubles.txt");
        await Assert.That(reader["Lorem"].AsDouble()).IsEqualTo(1.23);
        await Assert.That(reader["Lorem2"].AsDouble()).IsEqualTo(2.23);
        await Assert.That(reader["Lorem3"].AsDouble()).IsEqualTo(-3.23);
        await Assert.That(reader["Lorem4"].AsDouble()).IsEqualTo(4.23);
        await Assert.That(reader["Lorem5"].AsDouble()).IsEqualTo(5.23);
    }

    [Test]
    public async Task FiveBoolsTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/AllBools.txt");
        await Assert.That(reader["Lorem"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Lorem2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["Lorem3"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Lorem4"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["Lorem5"].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task MixedValues() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/Mixed.txt");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Test string");
        await Assert.That(reader["Lorem2"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Lorem3"].AsDouble()).IsEqualTo(3.25);
        await Assert.That(reader["Lorem4"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Lorem5"].AsInt()).IsEqualTo(-25);
    }

    [Test]
    public async Task ErrorLinesDisplayCorrectly() {
        await Assert.That(() => new TXTConfigFile("../../../data/TXT/5LineTests/Error.txt"))
            .Throws<FormatException>()
            .WithMessage("More than 1 split character was found at line 4. Please remove it or change your character to something else.");
    }

    [Test]
    public async Task BlankLinesAreSkipped() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/5LineTests/WithSpaces.txt");
        await Assert.That(reader["Lorem"].AsString()).IsEqualTo("Test string");
        await Assert.That(reader["Lorem2"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Lorem3"].AsDouble()).IsEqualTo(3.25);
        await Assert.That(reader["Lorem4"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Lorem5"].AsInt()).IsEqualTo(-25);
    }
}
public class SingleLineArrayTests {
    [Test]
    public async Task StringsAreParsed() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/SingleLineArrayTests/Strings.txt");

        await Assert.That(reader["String Array"][0].AsString()).IsEqualTo("Lorem 1");
        await Assert.That(reader["String Array"][1].AsString()).IsEqualTo("Lorem 2");
        await Assert.That(reader["String Array"][2].AsString()).IsEqualTo("Lorem 3");
        await Assert.That(reader["String Array"][3].AsString()).IsEqualTo("Lorem 4");
        await Assert.That(reader["String Array"][4].AsString()).IsEqualTo("Lorem 5");
    }

    [Test]
    public async Task IntsAreParsed() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/SingleLineArrayTests/Ints.txt");
        await Assert.That(reader["Some Ints"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader["Some Ints"][1].AsInt()).IsEqualTo(2);
        await Assert.That(reader["Some Ints"][2].AsInt()).IsEqualTo(3);
        await Assert.That(reader["Some Ints"][3].AsInt()).IsEqualTo(4);
        await Assert.That(reader["Some Ints"][4].AsInt()).IsEqualTo(5);
    }

    [Test]
    public async Task DoublesAreParsed() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/SingleLineArrayTests/Doubles.txt");
        await Assert.That(reader["Some Doubles"][0].AsDouble()).IsEqualTo(5.1);
        await Assert.That(reader["Some Doubles"][1].AsDouble()).IsEqualTo(8.0);
        await Assert.That(reader["Some Doubles"][2].AsDouble()).IsEqualTo(7.4);
        await Assert.That(reader["Some Doubles"][3].AsDouble()).IsEqualTo(8.6);
        await Assert.That(reader["Some Doubles"][4].AsDouble()).IsEqualTo(9.4);
    }

    [Test]
    public async Task BoolsAreParsed() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/SingleLineArrayTests/Bools.txt");
        await Assert.That(reader["Some Bools"][0].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Some Bools"][1].AsBool()).IsEqualTo(false);
        await Assert.That(reader["Some Bools"][2].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Some Bools"][3].AsBool()).IsEqualTo(false);
        await Assert.That(reader["Some Bools"][4].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task MixedArrayIsParsed() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/SingleLineArrayTests/Mixed.txt");
        await Assert.That(reader["Mixed Array"][0].AsString()).IsEqualTo("String 1");
        await Assert.That(reader["Mixed Array"][1].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Mixed Array"][2].AsDouble()).IsEqualTo(3.25);
        await Assert.That(reader["Mixed Array"][3].AsBool()).IsEqualTo(true);
    }
}
public class MultiLineArrayTests {
    [Test]
    public async Task TwoLineArrayTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/MultiLineArrayTests/TwoLineArray.txt");
        await Assert.That(reader["Two Line Array"][0].AsString()).IsEqualTo("Line 1");
        await Assert.That(reader["Two Line Array"][1].AsInt()).IsEqualTo(14);
        await Assert.That(reader["Two Line Array"][2].AsString()).IsEqualTo("Line 2");
        await Assert.That(reader["Two Line Array"][3].AsDouble()).IsEqualTo(3.25);
        await Assert.That(reader["Two Line Array"][4].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Two Line Array"][5].AsInt()).IsEqualTo(46);
    }

    [Test]
    public async Task FourLineArrayTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/MultiLineArrayTests/FourLineArray.txt");
        await Assert.That(reader["Four Line Array"][0].AsString()).IsEqualTo("Line 1");
        await Assert.That(reader["Four Line Array"][1].AsInt()).IsEqualTo(14);
        await Assert.That(reader["Four Line Array"][2].AsString()).IsEqualTo("Line 2");
        await Assert.That(reader["Four Line Array"][3].AsDouble()).IsEqualTo(3.25);
        await Assert.That(reader["Four Line Array"][4].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Four Line Array"][5].AsInt()).IsEqualTo(46);
    }

    [Test]
    public async Task UnclosedArrayThrowsError() {
        await Assert.That(() => new TXTConfigFile("../../../data/TXT/MultiLineArrayTests/Unclosed.txt"))
            .Throws<FormatException>()
            .WithMessage("Error parsing array around line 3. Please ensure you have a closing array brace.");
    }

    [Test]
    public async Task UnclosedArrayStopsBeforeNextKVP() {
        await Assert.That(() => new TXTConfigFile("../../../data/TXT/MultiLineArrayTests/Unclosed2.txt"))
            .Throws<FormatException>()
            .WithMessage("Error parsing array around line 3. Please ensure you have a closing array brace.");
    }
}
public class RealisticTests {
    [Test]
    public async Task FiveStringTest() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/Realistic/File1.txt");
        await Assert.That(reader["Enemy Name"].AsString()).IsEqualTo("Bad Guy");
        await Assert.That(reader["Enemy Color"].AsString()).IsEqualTo("#9645ff");
        await Assert.That(reader["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Enemy Speed"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Enemy Damage"].AsDouble()).IsEqualTo(23.4);
    }

    [Test]
    public async Task AIGeneratedSlop1() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/Realistic/File2.txt");
        await Assert.That(reader["name"].AsString()).IsEqualTo("John Doe");
        await Assert.That(reader["age"].AsInt()).IsEqualTo(27);
        await Assert.That(reader["height"].AsDouble()).IsEqualTo(5.9);
        await Assert.That(reader["weight"].AsInt()).IsEqualTo(175);
        await Assert.That(reader["is student"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["favorite color"].AsString()).IsEqualTo("Blue");
        await Assert.That(reader["has pet"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["city"].AsString()).IsEqualTo("Seattle");
        await Assert.That(reader["temperature"].AsDouble()).IsEqualTo(72.5);
        await Assert.That(reader["has driving license"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["zip code"].AsInt()).IsEqualTo(98101);
        await Assert.That(reader["is employed"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["salary"].AsDouble()).IsEqualTo(55000.50);
        await Assert.That(reader["likes coffee"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["hobbies"].AsString()).IsEqualTo("Reading");
    }

    [Test]
    public async Task File2AUsefulError() {
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/Realistic/File2.txt");
        await Assert.That(() => reader["Enemy Name"].AsString())
            .Throws<KeyNotFoundException>()
            .WithMessage("Object does not contain key \"Enemy Name\".\n\tPath: File2.txt");
    }

    [Test]
    public async Task AIGeneratedSlop2() {
        // Intentional error for closing commas
        TXTConfigFile reader = new TXTConfigFile("../../../data/TXT/Realistic/File3.txt");
        await Assert.That(reader["info"][0].AsString()).IsEqualTo("John Doe");
        await Assert.That(reader["info"][1].AsInt()).IsEqualTo(29);
        await Assert.That(reader["info"][2].AsDouble()).IsEqualTo(6.1);
        await Assert.That(reader["info"][3].AsBool()).IsEqualTo(true);

        await Assert.That(reader["data"][0].AsString()).IsEqualTo("True");
        await Assert.That(reader["data"][1].AsInt()).IsEqualTo(42);
        await Assert.That(reader["data"][2].AsString()).IsEqualTo("Seattle");

        await Assert.That(reader["preferences"][0].AsString()).IsEqualTo("Travel");
        await Assert.That(reader["preferences"][1].AsString()).IsEqualTo("Music");
        await Assert.That(reader["preferences"][2].AsDouble()).IsEqualTo(3.14);

        await Assert.That(() => reader["data"][3])
            .Throws<IndexOutOfRangeException>();
    }
}
