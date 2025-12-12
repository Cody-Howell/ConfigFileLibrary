using HowlDev.IO.Text.Parsers.Helpers;
namespace HowlDev.IO.Text.Parsers.Tests.HelperTests;

public class FirstOrderYAMLHelperTests {
    [Test]
    public async Task ArrayTest() {
        List<(int, string)> vals = YAMLHelper.ReturnOrderedLines(File.ReadAllText("../../../../HowlDev.IO.Text.ConfigFile.Tests/data/YAML/FirstOrder/Array.yaml"));
        await Assert.That(vals[0].Item1).IsEqualTo(0);
        await Assert.That(vals[0].Item2).IsEqualTo("- Test String");
        await Assert.That(vals[1].Item1).IsEqualTo(0);
        await Assert.That(vals[1].Item2).IsEqualTo("- 15");
        await Assert.That(vals[2].Item1).IsEqualTo(0);
        await Assert.That(vals[2].Item2).IsEqualTo("- 3.15");
        await Assert.That(vals[3].Item1).IsEqualTo(0);
        await Assert.That(vals[3].Item2).IsEqualTo("- true");
    }

    [Test]
    public async Task ObjectTest() {
        List<(int, string)> vals = YAMLHelper.ReturnOrderedLines(File.ReadAllText("../../../../HowlDev.IO.Text.ConfigFile.Tests/data/YAML/FirstOrder/Object.yaml"));
        await Assert.That(vals[0].Item1).IsEqualTo(0);
        await Assert.That(vals[0].Item2).IsEqualTo("Lorem: Test String");
        await Assert.That(vals[1].Item1).IsEqualTo(0);
        await Assert.That(vals[1].Item2).IsEqualTo("Lorem2: 15");
        await Assert.That(vals[2].Item1).IsEqualTo(0);
        await Assert.That(vals[2].Item2).IsEqualTo("Lorem3: 3.15");
        await Assert.That(vals[3].Item1).IsEqualTo(0);
        await Assert.That(vals[3].Item2).IsEqualTo("Lorem4: true");
    }
}
public class SecondOrderYAMLHelperTests {
    [Test]
    public async Task SimpleObjectTest() {
        List<(int, string)> vals = YAMLHelper.ReturnOrderedLines(File.ReadAllText("../../../../HowlDev.IO.Text.ConfigFile.Tests/data/YAML/SecondOrder/ObjectWithObject.yaml"));
        await Assert.That(vals[0].Item1).IsEqualTo(0);
        await Assert.That(vals[0].Item2).IsEqualTo("first:");
        await Assert.That(vals[1].Item1).IsEqualTo(1);
        await Assert.That(vals[1].Item2).IsEqualTo("lorem: test");
        await Assert.That(vals[2].Item1).IsEqualTo(1);
        await Assert.That(vals[2].Item2).IsEqualTo("num: 1");
        await Assert.That(vals[3].Item1).IsEqualTo(1);
        await Assert.That(vals[3].Item2).IsEqualTo("double: 2.0");
        await Assert.That(vals[4].Item1).IsEqualTo(1);
        await Assert.That(vals[4].Item2).IsEqualTo("bool: true");
        await Assert.That(vals[5].Item1).IsEqualTo(0);
        await Assert.That(vals[5].Item2).IsEqualTo("second:");
        await Assert.That(vals[6].Item1).IsEqualTo(1);
        await Assert.That(vals[6].Item2).IsEqualTo("ipsum: something");
        await Assert.That(vals[7].Item1).IsEqualTo(1);
        await Assert.That(vals[7].Item2).IsEqualTo("num: 2");
        await Assert.That(vals[8].Item1).IsEqualTo(1);
        await Assert.That(vals[8].Item2).IsEqualTo("double: 3.2");
        await Assert.That(vals[9].Item1).IsEqualTo(1);
        await Assert.That(vals[9].Item2).IsEqualTo("bool: false");
    }
}
public class ComplexObjectYAMLHelperTests {
    [Test]
    public async Task ComplexObjectTest() {
        List<(int, string)> vals = YAMLHelper.ReturnOrderedLines(File.ReadAllText("../../../../HowlDev.IO.Text.ConfigFile.Tests/data/YAML/Realistic/ComplexObject.yaml"));

        await Assert.That(vals[0].Item1).IsEqualTo(0);
        await Assert.That(vals[0].Item2).IsEqualTo("first:");
        await Assert.That(vals[1].Item1).IsEqualTo(1);
        await Assert.That(vals[1].Item2).IsEqualTo("simple Array:");
        await Assert.That(vals[2].Item1).IsEqualTo(2);
        await Assert.That(vals[2].Item2).IsEqualTo("- 1");
        await Assert.That(vals[3].Item1).IsEqualTo(2);
        await Assert.That(vals[3].Item2).IsEqualTo("- 2");
        await Assert.That(vals[4].Item1).IsEqualTo(2);
        await Assert.That(vals[4].Item2).IsEqualTo("- 3");
        await Assert.That(vals[5].Item1).IsEqualTo(1);
        await Assert.That(vals[5].Item2).IsEqualTo("brother: sample String");
        await Assert.That(vals[6].Item1).IsEqualTo(1);
        await Assert.That(vals[6].Item2).IsEqualTo("other sibling:");
        await Assert.That(vals[7].Item1).IsEqualTo(2);
        await Assert.That(vals[7].Item2).IsEqualTo("sibKey: sibValue");
        await Assert.That(vals[8].Item1).IsEqualTo(0);
        await Assert.That(vals[8].Item2).IsEqualTo("second:");
        await Assert.That(vals[9].Item1).IsEqualTo(1);
        await Assert.That(vals[9].Item2).IsEqualTo("arrayOfObjects:");
        await Assert.That(vals[10].Item1).IsEqualTo(2);
        await Assert.That(vals[10].Item2).IsEqualTo("- lorem: ipsum");
        await Assert.That(vals[11].Item1).IsEqualTo(2);
        await Assert.That(vals[11].Item2).IsEqualTo("something: 1.2");
        await Assert.That(vals[12].Item1).IsEqualTo(2);
        await Assert.That(vals[12].Item2).IsEqualTo("- lorem2: ipsum2");
        await Assert.That(vals[13].Item1).IsEqualTo(2);
        await Assert.That(vals[13].Item2).IsEqualTo("something2: false");
        await Assert.That(vals[14].Item1).IsEqualTo(1);
        await Assert.That(vals[14].Item2).IsEqualTo("otherThing: hopefully");
    }
}