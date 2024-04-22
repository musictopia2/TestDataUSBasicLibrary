namespace TestDataUSBasicLibrary.Extensions;
internal static class ExtensionsForCharacters
{
    public static string GetString(this IBasicList<char> list)
    {
        StrCat cats = new();
        foreach (char c in list)
        {
            cats.AddToString(c.ToString());
        }
        return cats.GetInfo();
    }
}
