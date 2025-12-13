using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.AsConstructedTests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests.AsConstructedTests;

public class AsConstructedTests
{
    [Test]
    public async Task PersonRecordTest()
    {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsConstructed<PersonRecord>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassTest()
    {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassTestForMissingInformation1()
    {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(0); // Default
    }

    [Test]
    public async Task PersonClassTestForMissingInformation2()
    {
        string txt = """
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo(string.Empty);
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassTestIgnoresExtraInformation()
    {
        string txt = """
        name: Jane
        id: 23
        lorem: empty
        irrelevant: ignored
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassTestIgnoresCaseForConstruction()
    {
        string txt = """
        Name: Jane
        Id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonRecordThrowsErrorWithoutAllParameters()
    {
        string txt = """
        name: Jane
        idx: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsConstructed<PersonRecord>()).Throws<InvalidOperationException>();
    }
}
