namespace HowlDev.IO.Text.ConfigFile.Tests.Classes;


public class BookClass {
    public string name { get; set; } = string.Empty;
    public double weight { get; set; }
    public double height { get; set; }
}

public class BrokenBookClass {
    public string Name { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double Height { get; set; }

    public BrokenBookClass() { }

    public BrokenBookClass(string innerName, double weight, double height) {
        Name = innerName;
        Weight = weight;
        Height = height;
    }

}