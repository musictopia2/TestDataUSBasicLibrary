namespace TestDataUSBasicLibrary;
/// <summary>
/// The main database object that can access data.
/// </summary>
internal static class Database
{
    /// <summary>
    /// The root of all data in a single object
    /// </summary>
    public static Lazy<BObject> BasicData { get; } = new(InitializeBasicData, LazyThreadSafetyMode.ExecutionAndPublication);


    //if i have anything else, then will do here as well.
    //this is only the basics but can be others too.


    /// <summary>
    /// Initializes the database.
    /// </summary>
    private static BObject InitializeBasicData()
    {
        string data = rr1.GetJsonText();
        BObject output = rr3.GetBsonFromJson(data);
        return output;
    }
    /// <summary>
    /// Returns the JToken of the locale category path. If the key does not exist, then the locale fallback is used.
    /// </summary>
    public static BValue? Get(string category, string path)
    {
        //var l = GetLocale(locale);

        var val = Select(category, path);
        return val;
    }
    public static BObject GetBasicData()
    {
        return BasicData.Value;
    }
    private static BValue? Select(string category, string path)
    {
        var data = GetBasicData();
        category = category.ToLowerInvariant();
        var section = data[category]; //hopefully this is okay (?)
        if (section is null)
        {
            return null;
        }

        var current = 0;
        while (true)
        {
            var len = path.IndexOf('.', current);

            string key;

            if (len < 0)
            {
                //dot in path not found, final key
                key = path.Substring(current);
                return section[key];
            }
            key = path.Substring(current, len);
            section = section[key];
            if (section is null)
            {
                return null;
            }
            current = len + 1;
        }
    }

}