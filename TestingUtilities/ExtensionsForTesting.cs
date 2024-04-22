namespace TestDataUSBasicLibrary.TestingUtilities;
public static class ExtensionsForTesting
{
    public static void Dump(this object obj)
    {
        Console.WriteLine(obj.DumpString());
    }
    public static string DumpString(this object obj)
    {
        return jj1.SerializeObject(obj);
    }
    //looks like you have to create a separate one for unit testing.

}