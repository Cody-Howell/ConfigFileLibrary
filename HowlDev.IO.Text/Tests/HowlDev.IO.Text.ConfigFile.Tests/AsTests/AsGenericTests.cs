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
}