namespace TestDataUSBasicLibrary.Extensions;
internal static class ExtensionsForCharacters
{
    extension (IBasicList<char> list)
    {
        public string GetString()
        {
            StrCat cats = new();
            foreach (char c in list)
            {
                cats.AddToString(c.ToString());
            }
            return cats.GetInfo();
        }
    }
}
