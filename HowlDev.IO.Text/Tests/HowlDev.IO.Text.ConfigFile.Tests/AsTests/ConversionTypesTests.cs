using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.Classes;

namespace HowlDev.IO.Text.ConfigFile.Tests.AsTests;

internal class ConversionTypesTests {
    [Test]
    public async Task ItAllWorks() {
        // Make a txt file that includes all the types a Primitive can parse to
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        PersonRecord p = reader.AsConstructed<PersonRecord>();
        await Assert.That(p.name).IsEqualTo("Jane");
        await Assert.That(p.id).IsEqualTo(23);
    }
}
