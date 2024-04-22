using TestDataUSBasicLibrary.SourceGeneratorHelpers;

namespace TestDataUSBasicLibrary;
public static class TestGeneratorHelpersGlobal<T>
    where T : class
{
    public static IMapPropertiesForTesting<T>? MasterContext { get; set; }
}