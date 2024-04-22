namespace TestDataUSBasicLibrary.SourceGeneratorHelpers;
public class DataSetMethodModel
{
    public string Name { get; set; } = "";
    public int OptionalArgumentsCount { get; set; }
    public int TotalArgumentsCount { get; set; }
    public Func<object, BasicList<string>, string>? Invoke { get; set; }
}