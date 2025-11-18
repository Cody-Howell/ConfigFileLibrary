namespace ConfigFileLibrary.Tests.FileCollector;

public class FileCollectorTests {
    [Test]
    public async Task NoExtensionThrowsException() {
        await Assert.That(() => new ConfigFileCollector(["lorem"]))
            .Throws<FormatException>()
            .WithMessage("File lorem does not have an extension.");
    }

    [Test]
    public async Task WrongExtensionThrowsException() {
        await Assert.That(() => new ConfigFileCollector(["lorem.haha"]))
            .Throws<FormatException>()
            .WithMessage("Extension not recognized: .haha");
    }
}
public class TXTFileCollectorTests {
    [Test]
    public async Task CanImportOneTXTFileAndReadIt() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../TXT/Realistic/File1.txt"]);

        TXTConfigFile reader = c.GetTXTFile("File1");
        await Assert.That(reader["Enemy Name"].AsString()).IsEqualTo("Bad Guy");
        await Assert.That(reader["Enemy Color"].AsString()).IsEqualTo("#9645ff");
        await Assert.That(reader["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Enemy Speed"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Enemy Damage"].AsDouble()).IsEqualTo(23.4);
    }

    [Test]
    public async Task CanImportTwoTXTFileAndReadThem() {
        ConfigFileCollector c = new ConfigFileCollector(
            ["../../../TXT/Realistic/File1.txt", "../../../TXT/SingleLineArrayTests/Ints.txt"]
            );

        TXTConfigFile reader1 = c.GetTXTFile("File1");
        await Assert.That(reader1["Enemy Name"].AsString()).IsEqualTo("Bad Guy");
        await Assert.That(reader1["Enemy Color"].AsString()).IsEqualTo("#9645ff");
        await Assert.That(reader1["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(reader1["Enemy Speed"].AsInt()).IsEqualTo(15);
        await Assert.That(reader1["Enemy Damage"].AsDouble()).IsEqualTo(23.4);

        TXTConfigFile reader2 = c.GetTXTFile("Ints");
        await Assert.That(reader2["Some Ints"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader2["Some Ints"][1].AsInt()).IsEqualTo(2);
        await Assert.That(reader2["Some Ints"][2].AsInt()).IsEqualTo(3);
        await Assert.That(reader2["Some Ints"][3].AsInt()).IsEqualTo(4);
        await Assert.That(reader2["Some Ints"][4].AsInt()).IsEqualTo(5);
    }

    [Test]
    public async Task TwoTXTFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../TXT/Realistic/File1.txt", "../../../TXT/Realistic/File1.txt"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownTXTFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../TXT/Realistic/File1.txt", "../../../TXT/Realistic/File2.txt"]);
        await Assert.That(() => c.GetTXTFile("File3"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tFile1\n\tFile2");
    }
}
public class YAMLFileCollectorTests {
    [Test]
    public async Task CanImportOneYAMLFileAndReadIt() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../YAML/Realistic/ComplexObject.yaml"]);

        YAMLConfigFile reader = c.GetYAMLFile("ComplexObject");
        await Assert.That(reader["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["second"]["otherThing"].AsString()).IsEqualTo("hopefully");
    }

    [Test]
    public async Task CanImportTwoYAMLFileAndReadThem() {
        ConfigFileCollector c = new ConfigFileCollector(
            ["../../../YAML/Realistic/ComplexObject.yaml", "../../../YAML/SecondOrder/MixedArray.yml"]
            );

        YAMLConfigFile reader1 = c.GetYAMLFile("ComplexObject");
        await Assert.That(reader1["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader1["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader1["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader1["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader1["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader1["second"]["otherThing"].AsString()).IsEqualTo("hopefully");

        YAMLConfigFile reader2 = c.GetYAMLFile("MixedArray");
        await Assert.That(reader2[0]["object"].AsString()).IsEqualTo("this is");
        await Assert.That(reader2[0]["part2"].AsString()).IsEqualTo("still part of this object");
        await Assert.That(reader2[0]["part3"].AsDouble()).IsEqualTo(45.3);
        await Assert.That(reader2[1].AsInt()).IsEqualTo(15);
        await Assert.That(reader2[2].AsString()).IsEqualTo("test string");
    }

    [Test]
    public async Task TwoYAMLFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../YAML/SecondOrder/MixedArray.yml", "../../../YAML/SecondOrder/MixedArray.yml"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownYAMLFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../YAML/SecondOrder/MixedArray.yml"]);
        await Assert.That(() => c.GetYAMLFile("File1"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tMixedArray");
    }
}
public class JSONFileCollectorTests {
    [Test]
    public async Task CanImportOneJSONFileAndReadIt() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../JSON/Realistic/ComplexObject.json"]);

        JSONConfigFile reader = c.GetJSONFile("ComplexObject");
        await Assert.That(reader["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader["second"]["otherThing"].AsString()).IsEqualTo("hopefully");
    }

    [Test]
    public async Task CanImportTwoJSONFileAndReadThem() {
        ConfigFileCollector c = new ConfigFileCollector(
            ["../../../JSON/Realistic/ComplexObject.json", "../../../JSON/SecondOrder/ArrayWithArray.json"]
            );

        JSONConfigFile reader1 = c.GetJSONFile("ComplexObject");
        await Assert.That(reader1["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader1["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader1["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader1["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader1["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader1["second"]["otherThing"].AsString()).IsEqualTo("hopefully");

        JSONConfigFile reader2 = c.GetJSONFile("ArrayWithArray");
        await Assert.That(reader2[0][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader2[0][1].AsBool()).IsEqualTo(true);
        await Assert.That(reader2[0][2].AsString()).IsEqualTo("string");

        await Assert.That(reader2[1][0].AsString()).IsEqualTo("second array");
        await Assert.That(reader2[1][1].AsDouble()).IsEqualTo(5.3);
        await Assert.That(reader2[1][2].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task TwoYAMLFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../JSON/SecondOrder/ArrayWithArray.json", "../../../JSON/SecondOrder/ArrayWithArray.json"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownYAMLFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../JSON/SecondOrder/ArrayWithArray.json"]);
        await Assert.That(() => c.GetJSONFile("File1"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tArrayWithArray");
    }
}
public class MixedTests {
    [Test]
    public async Task CanImportOneOfEach() {
        ConfigFileCollector c = new ConfigFileCollector([
            "../../../TXT/Realistic/File1.txt",
            "../../../YAML/Realistic/ComplexObject.yaml",
            "../../../JSON/Realistic/ComplexObject.json",
            ]);

        TXTConfigFile t = c.GetTXTFile("File1");
        YAMLConfigFile y = c.GetYAMLFile("ComplexObject");
        JSONConfigFile j = c.GetJSONFile("ComplexObject");

        await Assert.That(t["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(y["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");
        await Assert.That(j["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");
    }
}