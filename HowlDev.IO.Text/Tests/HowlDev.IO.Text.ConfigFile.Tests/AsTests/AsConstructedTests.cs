using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests;

public class AsConstructedTests {
    [Test]
    public async Task PersonRecordTest() {
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
    public async Task PersonClassTest() {
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
    public async Task PersonClassTestForMissingInformation1() {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(0); // Default
    }

    [Test]
    public async Task PersonClassTestForMissingInformation2() {
        string txt = """
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo(string.Empty);
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassTestForEmptyConstructor() {
        string txt = """
        idx: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo(string.Empty);
        await Assert.That(p.id).IsEqualTo(0);
    }

    [Test]
    public async Task PersonClassTestIgnoresExtraInformation() {
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
    public async Task PersonClassTestIgnoresCaseForConstruction1() {
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
    public async Task PersonClassTestIgnoresCaseForConstruction2() {
        string txt = """
        Name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonClass p = reader.AsConstructed<PersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonRecordThrowsErrorWithoutAllParameters() {
        string txt = """
        name: Jane
        idx: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsConstructed<PersonRecord>())
            .Throws<InvalidOperationException>()
            .WithMessage("""
                No suitable constructor found for PersonRecord. 
                Tried to find a constructor that matched the following keys: name, idx.
                """);
    }

    [Test]
    public async Task PersonRecordIgnoresExtraValues() {
        string txt = """
        name: Jane
        id: 23
        lorem: empty
        irrelevant: ignored
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsConstructed<PersonRecord>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }
}
public class AsConstructedStrictTests {
    [Test]
    public async Task PersonRecordTest() {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsStrictConstructed<PersonRecord>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonRecordFailsWhenNotEnoughInformation() {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictConstructed<PersonRecord>())
            .Throws<InvalidOperationException>()
            .WithMessage("""
                    No suitable constructor found for PersonRecord. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: name.
                    """);
    }

    [Test]
    public async Task PersonRecordFailsWhenTooMuchInformation() {
        string txt = """
        name: Jane
        id: 23
        lorem: breaks
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictConstructed<PersonRecord>())
            .Throws<StrictMappingException>()
            .WithMessage("""
                    No suitable constructor found for PersonRecord. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: name, id, lorem.
                    """);
    }

    [Test]
    public async Task PersonClassTest() {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        StrictPersonClass p = reader.AsStrictConstructed<StrictPersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }

    [Test]
    public async Task PersonClassFailsWhenNotEnoughInformation() {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictConstructed<StrictPersonClass>())
            .Throws<InvalidOperationException>()
            .WithMessage("""
                    No suitable constructor found for StrictPersonClass. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: name.
                    """);
    }

    [Test]
    public async Task PersonClassFailsWhenTooMuchInformation() {
        string txt = """
        name: Jane
        id: 23
        lorem: breaks
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictConstructed<StrictPersonClass>())
            .Throws<StrictMappingException>()
            .WithMessage("""
                    No suitable constructor found for StrictPersonClass. Consider removing the StrictMatching flag. 
                    Tried to find a constructor that matched the following keys: name, id, lorem.
                    """);
    }
}