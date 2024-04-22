namespace TestDataUSBasicLibrary.SourceGeneratorHelpers;
public class DataSetClassModel
{
    public string Name { get; set; } = "";
    public Type? DeclaringType { get; set; }
    //this means can be set later.
    public object? DataSet { get; set; }
    public BasicList<DataSetMethodModel> Methods { get; set; } = [];
}