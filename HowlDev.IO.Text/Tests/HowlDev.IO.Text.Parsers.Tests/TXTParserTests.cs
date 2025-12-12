using HowlDev.IO.Text.Parsers.Enums;

namespace HowlDev.IO.Text.Parsers.Tests;

internal class TXTParserTests
{
    [Test]
    public async Task File1()
    {
        List<(TextToken token, string value)> parsed = new (new TXTParser(File.ReadAllText("../../../data/TXT/File1.txt")));
    }
    
    [Test]
    public async Task String()
    {
        List<(TextToken token, string value)> parsed = new (new TXTParser(File.ReadAllText("../../../data/TXT/String.txt")));
    }

    [Test]
    public async Task MixedObject()
    {
        List<(TextToken token, string value)> parsed = new (new TXTParser(File.ReadAllText("../../../data/TXT/MixedObject.txt")));
    }

    [Test]
    public async Task MixedArray()
    {
        List<(TextToken token, string value)> parsed = new (new TXTParser(File.ReadAllText("../../../data/TXT/MixedArray.txt")));
    }

    [Test]
    public async Task FourLineArray()
    {
        List<(TextToken token, string value)> parsed = new (new TXTParser(File.ReadAllText("../../../data/TXT/FourLineArray.txt")));
    }
}
