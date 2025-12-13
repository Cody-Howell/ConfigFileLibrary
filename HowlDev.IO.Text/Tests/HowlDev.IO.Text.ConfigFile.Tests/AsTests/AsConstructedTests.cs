using HowlDev.IO.Text.ConfigFile.Enums;
using HowlDev.IO.Text.ConfigFile.Tests.AsConstructedTests.Classes;
namespace HowlDev.IO.Text.ConfigFile.Tests.AsConstructedTests;

public class AsConstructedTests
{
    [Test]
    public async Task PersonTest()
    {
        string txt = """
        name: Jane
        id: 23
        """;
        TextConfigFile reader = TextConfigFile.ReadTextAs(FileTypes.TXT, txt);

        Person p = reader.AsConstructed<Person>();
    }
}
