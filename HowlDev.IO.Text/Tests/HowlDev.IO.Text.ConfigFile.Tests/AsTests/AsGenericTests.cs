using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests;

public class AsGenericTests {
    [Test]
    public async Task PersonRecordTest() {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.As<PersonRecord>();
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

        await Assert.That(() => reader.As<PersonRecord>())
            .Throws<InvalidOperationException>()
            .WithMessage("Type PersonRecord must have a parameterless constructor to use property mapping.");
    }

    [Test]
    public async Task PersonClassCanBypassInvalidConstructors() {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        StrictPersonClass p = reader.As<StrictPersonClass>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(0);
    }
}

public class AsGenericStrictTests {
    [Test]
    public async Task PersonRecordTest() {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsStrict<PersonRecord>();
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
        await Assert.That(() => reader.AsStrict<PersonRecord>())
            .Throws<InvalidOperationException>()
            .WithMessage("""
                        Type PersonRecord must have a parameterless constructor to use property mapping.
                        """);
    }

    [Test]
    public async Task PersonClassCanBypassInvalidConstructors() {
        string txt = """
        name: Jane
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrict<StrictPersonClass>())
            .Throws<StrictMappingException>()
            .WithMessage("""
                        Property count mismatch for StrictPersonClass. Consider removing the StrictMatching flag. 
                        Property count of type: 2. Key count of object: 1.
                        """);
    }

    [Test]
    public async Task BrokenBookCanBypassBadConstructors() {
        string txt = """
        name: Little Women
        weight: 2.3
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BrokenBookClass b = reader.AsStrict<BrokenBookClass>();
        await Assert.That(b.Name).IsEqualTo("Little Women");
        await Assert.That(b.Weight).IsEqualTo(2.3);
        await Assert.That(b.Height).IsEqualTo(12.3);
    }
}