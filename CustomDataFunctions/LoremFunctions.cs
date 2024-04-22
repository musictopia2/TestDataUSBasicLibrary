namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class LoremFunctions<T>
    where T: Lorem
{
    public static Func<T, string> Word { get; set; } = lorem =>
    {
        return lorem.GetRandomListItem("words");
    };
}