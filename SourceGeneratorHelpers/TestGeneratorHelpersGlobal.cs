namespace TestDataUSBasicLibrary.SourceGeneratorHelpers;
public static class TestGeneratorHelpersGlobal<T>
    where T : class
{
    public static IMapPropertiesForTesting<T>? MasterContext { get; set; }
}