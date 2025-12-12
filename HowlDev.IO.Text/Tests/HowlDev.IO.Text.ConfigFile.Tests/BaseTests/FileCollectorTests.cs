using HowlDev.IO.Text.ConfigFile;

namespace HowlDev.IO.Text.ConfigFile.Tests.BaseTests;

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
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/TXT/Realistic/File1.txt"]);

        TextConfigFile reader = c.GetFile("File1.txt");
        await Assert.That(reader["Enemy Name"].AsString()).IsEqualTo("Bad Guy");
        await Assert.That(reader["Enemy Color"].AsString()).IsEqualTo("#9645ff");
        await Assert.That(reader["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(reader["Enemy Speed"].AsInt()).IsEqualTo(15);
        await Assert.That(reader["Enemy Damage"].AsDouble()).IsEqualTo(23.4);
    }

    [Test]
    public async Task CanImportTwoTXTFileAndReadThem() {
        ConfigFileCollector c = new ConfigFileCollector(
            ["../../../data/TXT/Realistic/File1.txt", "../../../data/TXT/SingleLineArrayTests/Ints.txt"]
            );

        TextConfigFile reader1 = c.GetFile("File1.txt");
        await Assert.That(reader1["Enemy Name"].AsString()).IsEqualTo("Bad Guy");
        await Assert.That(reader1["Enemy Color"].AsString()).IsEqualTo("#9645ff");
        await Assert.That(reader1["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(reader1["Enemy Speed"].AsInt()).IsEqualTo(15);
        await Assert.That(reader1["Enemy Damage"].AsDouble()).IsEqualTo(23.4);

        TextConfigFile reader2 = c.GetFile("Ints.txt");
        await Assert.That(reader2["Some Ints"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader2["Some Ints"][1].AsInt()).IsEqualTo(2);
        await Assert.That(reader2["Some Ints"][2].AsInt()).IsEqualTo(3);
        await Assert.That(reader2["Some Ints"][3].AsInt()).IsEqualTo(4);
        await Assert.That(reader2["Some Ints"][4].AsInt()).IsEqualTo(5);
    }

    [Test]
    public async Task TwoTXTFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../data/TXT/Realistic/File1.txt", "../../../data/TXT/Realistic/File1.txt"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownTXTFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/TXT/Realistic/File1.txt", "../../../data/TXT/Realistic/File2.txt"]);
        await Assert.That(() => c.GetFile("File3.txt"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tFile1.txt\n\tFile2.txt");
    }
}
public class YAMLFileCollectorTests {
    [Test]
    public async Task CanImportOneYAMLFileAndReadIt() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/YAML/Realistic/ComplexObject.yaml"]);

        TextConfigFile reader = c.GetFile("ComplexObject.yaml");
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
            ["../../../data/YAML/Realistic/ComplexObject.yaml", "../../../data/YAML/SecondOrder/MixedArray.yml"]
            );

        TextConfigFile reader1 = c.GetFile("ComplexObject.yaml");
        await Assert.That(reader1["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader1["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader1["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader1["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader1["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader1["second"]["otherThing"].AsString()).IsEqualTo("hopefully");

        TextConfigFile reader2 = c.GetFile("MixedArray.yml");
        await Assert.That(reader2[0]["object"].AsString()).IsEqualTo("this is");
        await Assert.That(reader2[0]["part2"].AsString()).IsEqualTo("still part of this object");
        await Assert.That(reader2[0]["part3"].AsDouble()).IsEqualTo(45.3);
        await Assert.That(reader2[1].AsInt()).IsEqualTo(15);
        await Assert.That(reader2[2].AsString()).IsEqualTo("test string");
    }

    [Test]
    public async Task TwoYAMLFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../data/YAML/SecondOrder/MixedArray.yml", "../../../data/YAML/SecondOrder/MixedArray.yml"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownYAMLFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/YAML/SecondOrder/MixedArray.yml"]);
        await Assert.That(() => c.GetFile("File1.txt"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tMixedArray.yml");
    }
}
public class JSONFileCollectorTests {
    [Test]
    public async Task CanImportOneJSONFileAndReadIt() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/JSON/Realistic/ComplexObject.json"]);

        TextConfigFile reader = c.GetFile("ComplexObject.json");
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
            ["../../../data/JSON/Realistic/ComplexObject.json", "../../../data/JSON/SecondOrder/ArrayWithArray.json"]
            );

        TextConfigFile reader1 = c.GetFile("ComplexObject.json");
        await Assert.That(reader1["first"]["simple Array"][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader1["first"]["brother"].AsString()).IsEqualTo("sample String");
        await Assert.That(reader1["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");

        await Assert.That(reader1["second"]["arrayOfObjects"][0]["lorem"].AsString()).IsEqualTo("ipsum");
        await Assert.That(reader1["second"]["arrayOfObjects"][1]["something2"].AsBool()).IsEqualTo(false);
        await Assert.That(reader1["second"]["otherThing"].AsString()).IsEqualTo("hopefully");

        TextConfigFile reader2 = c.GetFile("ArrayWithArray.json");
        await Assert.That(reader2[0][0].AsInt()).IsEqualTo(1);
        await Assert.That(reader2[0][1].AsBool()).IsEqualTo(true);
        await Assert.That(reader2[0][2].AsString()).IsEqualTo("string");

        await Assert.That(reader2[1][0].AsString()).IsEqualTo("second array");
        await Assert.That(reader2[1][1].AsDouble()).IsEqualTo(5.3);
        await Assert.That(reader2[1][2].AsBool()).IsEqualTo(false);
    }

    [Test]
    public async Task TwoYAMLFilesWithSameNameAndExtensionThrowError() {
        await Assert.That(() => new ConfigFileCollector(["../../../data/JSON/SecondOrder/ArrayWithArray.json", "../../../data/JSON/SecondOrder/ArrayWithArray.json"]))
            .Throws<NotSupportedException>()
            .WithMessage("Cannot add in two filenames of the same name and extension.");
    }

    [Test]
    public async Task RetrievingUnknownYAMLFileThrowsHelpfulError() {
        ConfigFileCollector c = new ConfigFileCollector(["../../../data/JSON/SecondOrder/ArrayWithArray.json"]);
        await Assert.That(() => c.GetFile("File1.txt"))
            .Throws<FileNotFoundException>()
            .WithMessage("Filename does not exist. Available keys: \n\tArrayWithArray.json");
    }
}
public class MixedFileCollectorTests {
    [Test]
    public async Task CanImportOneOfEach() {
        ConfigFileCollector c = new ConfigFileCollector([
            "../../../data/TXT/Realistic/File1.txt",
            "../../../data/YAML/Realistic/ComplexObject.yaml",
            "../../../data/JSON/Realistic/ComplexObject.json",
            ]);

        TextConfigFile t = c.GetFile("File1.txt");
        TextConfigFile y = c.GetFile("ComplexObject.yaml");
        TextConfigFile j = c.GetFile("ComplexObject.json");

        await Assert.That(t["Is Boss"].AsBool()).IsEqualTo(true);
        await Assert.That(y["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");
        await Assert.That(j["first"]["other sibling"]["sibKey"].AsString()).IsEqualTo("sibValue");
    }
}