using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests;

public class AsPropertiesTests {
    [Test]
    public async Task PersonRecordTest() {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsProperties<PersonRecord>())
            .Throws<InvalidOperationException>()
            .WithMessage("Type PersonRecord must have a parameterless constructor to use property mapping.");
    }

    [Test]
    public async Task BookClassTest() {
        string txt = """
        name: Little Women
        weight: 2.3
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BookClass b = reader.AsProperties<BookClass>();
        await Assert.That(b.name).IsEqualTo("Little Women");
        await Assert.That(b.weight).IsEqualTo(2.3);
        await Assert.That(b.height).IsEqualTo(12.3);
    }

    [Test]
    public async Task BookClassDefaultsMissingInformation1() {
        string txt = """
        name: Little Women
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BookClass b = reader.AsProperties<BookClass>();
        await Assert.That(b.name).IsEqualTo("Little Women");
        await Assert.That(b.weight).IsEqualTo(0);
        await Assert.That(b.height).IsEqualTo(12.3);
    }

    [Test]
    public async Task BookClassDefaultsMissingInformation2() {
        string txt = """
        weight: 2.3
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BookClass b = reader.AsProperties<BookClass>();
        await Assert.That(b.name).IsEqualTo(string.Empty);
        await Assert.That(b.weight).IsEqualTo(2.3);
        await Assert.That(b.height).IsEqualTo(12.3);
    }

    [Test]
    public async Task BookClassDefaultsMissingInformation3() {
        string txt = """
        name: Little Women
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BookClass b = reader.AsProperties<BookClass>();
        await Assert.That(b.name).IsEqualTo("Little Women");
        await Assert.That(b.weight).IsEqualTo(0);
        await Assert.That(b.height).IsEqualTo(0);
    }
}
public class AsPropertiesStrictTests {
    [Test]
    public async Task BookClassTest() {
        string txt = """
        name: Little Women
        weight: 2.3
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        BookClass b = reader.AsStrictProperties<BookClass>();
        await Assert.That(b.name).IsEqualTo("Little Women");
        await Assert.That(b.weight).IsEqualTo(2.3);
        await Assert.That(b.height).IsEqualTo(12.3);
    }

    [Test]
    public async Task BookClassMismatchPropertyCounts1() {
        string txt = """
        name: Little Women
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictProperties<BookClass>())
            .Throws<StrictMappingException>()
            .WithMessage($"""
                        Property count mismatch for BookClass. Consider removing the StrictMatching flag. 
                        Property count of type: 3. Key count of object: 2.
                        """);
    }

    [Test]
    public async Task BookClassMismatchPropertyCounts2() {
        string txt = """
        name: Little Women
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictProperties<BookClass>())
            .Throws<StrictMappingException>()
            .WithMessage($"""
                        Property count mismatch for BookClass. Consider removing the StrictMatching flag. 
                        Property count of type: 3. Key count of object: 1.
                        """);
    }

    [Test]
    public async Task BookClassMismatchPropertyCounts3() {
        string txt = """
        name: Little Women
        weight: 2.3
        height: 12.3
        lorem: broken
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictProperties<BookClass>())
            .Throws<StrictMappingException>()
            .WithMessage($"""
                        Property count mismatch for BookClass. Consider removing the StrictMatching flag. 
                        Property count of type: 3. Key count of object: 4.
                        """);
    }

    [Test]
    public async Task BookClassMismatchPropertyNames1() {
        string txt = """
        name: Little Women
        weight: 2.3
        hieght: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictProperties<BookClass>())
            .Throws<StrictMappingException>()
            .WithMessage("""
                        Property mismatch for BookClass. Consider removing the StrictMatching flag. 
                        Could not find matching object key for property: height.
                        """);
    }

    [Test]
    public async Task BookClassMismatchPropertyNames2() {
        string txt = """
        nam: Little Women
        weight: 2.3
        height: 12.3
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        await Assert.That(() => reader.AsStrictProperties<BookClass>())
            .Throws<StrictMappingException>()
            .WithMessage("""
                        Property mismatch for BookClass. Consider removing the StrictMatching flag. 
                        Could not find matching object key for property: name.
                        """);
    }
}