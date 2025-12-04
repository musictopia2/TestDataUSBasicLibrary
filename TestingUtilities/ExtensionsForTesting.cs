namespace TestDataUSBasicLibrary.TestingUtilities;
public static class ExtensionsForTesting
{
    extension (object payLoad)
    {
        public void Dump()
        {
            Console.WriteLine(payLoad.DumpString());
        }
        public string DumpString()
        {
            return jj1.SerializeObject(payLoad);
        }
    }
}