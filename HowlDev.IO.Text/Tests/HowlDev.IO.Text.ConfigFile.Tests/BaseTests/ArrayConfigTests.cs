using HowlDev.IO.Text.ConfigFile.Primitives;
namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

public class FirstOrderArrayConfigTests {
    [Test]
    public async Task ArrayOfStrings() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("lorem"),
            new PrimitiveConfigOption("huh"),
            ]);
        await Assert.That(array[0].AsString()).IsEqualTo("True");
        await Assert.That(array[1].AsString()).IsEqualTo("lorem");
        await Assert.That(array[2].AsString()).IsEqualTo("huh");
    }

    [Test]
    public async Task ArrayOfStringsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("lorem"),
            new PrimitiveConfigOption("huh"),
            ]);
        List<string> testList = array.AsStringList();
        await Assert.That(testList.Count).IsEqualTo(3);
        await Assert.That(testList[0]).IsEqualTo("True");
        await Assert.That(testList[1]).IsEqualTo("lorem");
        await Assert.That(testList[2]).IsEqualTo("huh");
    }

    [Test]
    public async Task ArrayOfInts() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1"),
            new PrimitiveConfigOption("3"),
            new PrimitiveConfigOption("4"),
            new PrimitiveConfigOption("-5"),
            ]);
        await Assert.That(array[0].AsInt()).IsEqualTo(1);
        await Assert.That(array[1].AsInt()).IsEqualTo(3);
        await Assert.That(array[2].AsInt()).IsEqualTo(4);
        await Assert.That(array[3].AsInt()).IsEqualTo(-5);
    }

    [Test]
    public async Task ArrayOfIntsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1"),
            new PrimitiveConfigOption("3"),
            new PrimitiveConfigOption("4"),
            new PrimitiveConfigOption("-5"),
            ]);
        List<int> testList = array.AsIntList();
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(1);
        await Assert.That(testList[1]).IsEqualTo(3);
        await Assert.That(testList[2]).IsEqualTo(4);
        await Assert.That(testList[3]).IsEqualTo(-5);
    }

    [Test]
    public async Task ArrayOfDoubles() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1.234"),
            new PrimitiveConfigOption("3.74"),
            new PrimitiveConfigOption("4.1"),
            new PrimitiveConfigOption("-5.463"),
            ]);
        await Assert.That(array[0].AsDouble()).IsEqualTo(1.234);
        await Assert.That(array[1].AsDouble()).IsEqualTo(3.74);
        await Assert.That(array[2].AsDouble()).IsEqualTo(4.1);
        await Assert.That(array[3].AsDouble()).IsEqualTo(-5.463);
    }

    [Test]
    public async Task ArrayOfDoublesAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("1.234"),
            new PrimitiveConfigOption("3.74"),
            new PrimitiveConfigOption("4.1"),
            new PrimitiveConfigOption("-5.463"),
            ]);
        List<double> testList = array.AsDoubleList();
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(1.234);
        await Assert.That(testList[1]).IsEqualTo(3.74);
        await Assert.That(testList[2]).IsEqualTo(4.1);
        await Assert.That(testList[3]).IsEqualTo(-5.463);
    }

    [Test]
    public async Task ArrayOfBools() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("true"),
            new PrimitiveConfigOption("False"),
            new PrimitiveConfigOption("false"),
            ]);
        await Assert.That(array[0].AsBool()).IsEqualTo(true);
        await Assert.That(array[1].AsBool()).IsEqualTo(true);
        await Assert.That(array[2].AsBool()).IsEqualTo(false);
        await Assert.That(array[3].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task ArrayOfBoolsAsList() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("True"),
            new PrimitiveConfigOption("true"),
            new PrimitiveConfigOption("False"),
            new PrimitiveConfigOption("false"),
            ]);
        List<bool> testList = array.AsBoolList();
        await Assert.That(testList.Count).IsEqualTo(4);
        await Assert.That(testList[0]).IsEqualTo(true);
        await Assert.That(testList[1]).IsEqualTo(true);
        await Assert.That(testList[2]).IsEqualTo(false);
        await Assert.That(testList[3]).IsEqualTo(false);
    }

    [Test]
    public async Task TypeCastingThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        string exceptionValue = "Type casting not allowed on type ArrayConfigOption";
        await Assert.That(array.AsString)
            .Throws<InvalidOperationException>()
            .WithMessage(exceptionValue);
        await Assert.That(array.AsInt)
            .Throws<InvalidOperationException>()
            .WithMessage(exceptionValue);
        await Assert.That(array.AsDouble)
            .Throws<InvalidOperationException>()
            .WithMessage(exceptionValue);
        await Assert.That(array.AsBool)
            .Throws<InvalidOperationException>()
            .WithMessage(exceptionValue);
    }

    [Test]
    public async Task StringIndexingThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array["lorem"])
            .Throws<InvalidOperationException>()
            .WithMessage("Operation invalid on type of ArrayConfigOption.");
    }

    [Test]
    public async Task OutOfBoundsThrowsError() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            ]);
        await Assert.That(() => array[1])
            .Throws<IndexOutOfRangeException>()
            .WithMessage("Index 1 is out of range. This array has 1 items.");
    }

    [Test]
    public async Task MixedArray() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            new PrimitiveConfigOption("15"),
            new PrimitiveConfigOption("2.34"),
            new PrimitiveConfigOption("true")
            ]);
        await Assert.That(array[0].AsString()).IsEqualTo("Lorem");
        await Assert.That(array[1].AsInt()).IsEqualTo(15);
        await Assert.That(array[2].AsDouble()).IsEqualTo(2.34);
        await Assert.That(array[3].AsBool()).IsEqualTo(true);
    }

    [Test]
    public async Task MixedArrayThrowsAllErrorsAsSpecificLists() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("Lorem"),
            new PrimitiveConfigOption("15"),
            new PrimitiveConfigOption("2.34"),
            new PrimitiveConfigOption("true")
            ]);
        await Assert.That(array.AsIntList)
            .Throws<InvalidCastException>(); // Throws the exception from the PrimitiveConfigOption, tested elsewhere.
        await Assert.That(array.AsDoubleList)
            .Throws<InvalidCastException>();
        await Assert.That(array.AsBoolList)
            .Throws<InvalidCastException>();
    }
}
public class SecondOrderArrayConfigTests {
    [Test]
    public async Task ArrayOfArrays() {
        ArrayConfigOption array = new ArrayConfigOption([
            new PrimitiveConfigOption("15"),
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ]),
            new PrimitiveConfigOption("false")
            ]);
        await Assert.That(array[0].AsInt()).IsEqualTo(15);
        await Assert.That(array[1][0].AsString()).IsEqualTo("Lorem");
        await Assert.That(array[1][1].AsDouble()).IsEqualTo(2.34);
        await Assert.That(array[2].AsBool).IsEqualTo(false);
    }

    [Test]
    public async Task NestedArraysCantBeReturnedAsLists() {
        ArrayConfigOption array = new ArrayConfigOption([ // Order greatly matters here for what exception is thrown
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ]),
            new PrimitiveConfigOption("15"),
            new PrimitiveConfigOption("false")
            ]);
        await Assert.That(array.AsStringList)
            .Throws<InvalidOperationException>();
        await Assert.That(array.AsIntList)
            .Throws<InvalidOperationException>();
        await Assert.That(array.AsDoubleList)
            .Throws<InvalidOperationException>();
        await Assert.That(array.AsBoolList)
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task NestedArraysThrowHelpfulErrors() {
        ArrayConfigOption array = new ArrayConfigOption([
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ], "test", "0"),
            new ArrayConfigOption([
                new PrimitiveConfigOption("Lorem"),
                new PrimitiveConfigOption("2.34")
                ], "test", "1")
            ], "test");
        await Assert.That(() => array[0][2])
            .Throws<IndexOutOfRangeException>()
            .WithMessage("Index 2 is out of range. This array has 2 items.\n\tPath: test[0]");
    }
}