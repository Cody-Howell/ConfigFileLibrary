using TUnit.Engine.Services;

namespace HowlDev.IO.Text.ConfigFile.Tests.AsConstructedTests.Classes;


public record PersonRecord(string name, int id);
public class PersonClass
{
    public string name {get;set;} = string.Empty;
    public int id {get;set;}

    public PersonClass() {}

    public PersonClass(int id)
    {
        this.id = id;
    }

    public PersonClass(string name)
    {
        this.name = name;
    }

    public PersonClass(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}
