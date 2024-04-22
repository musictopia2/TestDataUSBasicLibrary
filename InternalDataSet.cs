namespace TestDataUSBasicLibrary;
public abstract partial class InternalDataSet : DataSet
{
    /// <summary>
    /// sets the category to use.  i propose making all use a name.  obviously if doing their own, set to blank.
    /// </summary>
    protected abstract string Category { get; }
    /// <summary>
    /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">path/key in the category</param>
    /// <returns>A BSON value for the given JSON path.</returns>
    protected internal virtual BValue? Get(string path)
    {
        return Database.Get(Category, path);
    }
    /// <summary>
    /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="category">Overrides the category name on the dataset.</param>
    /// <param name="path">path/key in the category.</param>
    /// <returns>A BSON value for the given JSON path.</returns>
    protected internal virtual BValue? Get(string category, string path)
    {
        return Database.Get(category, path);
    }
    /// <summary>
    /// Returns a BSON array given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">key in the category.</param>
    /// <returns>A BSON value for the given JSON path.</returns>
    protected internal virtual BCustomList? GetCustomList(string path)
    {
        return (BCustomList)Get(path)!;
    }

    protected internal virtual BCustomList? GetCustomList(string category, string path)
    {
        return (BCustomList)Get(category, path)!;
    }
    /// <summary>
    /// Returns a BSON object given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">path/key in the category</param>
    /// <returns>A BSON value for the given JSON path.</returns>
    protected internal virtual BObject? GetObject(string path)
    {
        return (BObject)Get(path)!;
    }
    /// <summary>
    /// Picks a random string inside a BSON array. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">key in the category</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>A random item from the BSON array.</returns>
    protected internal virtual string GetRandomListItem(string path, int? min = null, int? max = null)
    {
        return GetRandomListItem(Category, path, min, max);
    }
    //good news is this is overridable.  so a custom class can do something else to get a random list item based on information being passed
    protected internal virtual string GetRandomListItem(string category, string path, int? min = null, int? max = null)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new CustomBasicException("Cannot have a blank category");
        }
        var arr = GetCustomList(category, path);
        if (!arr!.HasValues)
        {
            return string.Empty;
        }

        return Random.ListItem(arr, min, max);
    }
    protected internal virtual IBasicList<string> GetRandomListCollection(string path, int howMany)
    {
        return GetRandomListCollection(Category, path, howMany);
    }
    protected internal virtual IBasicList<string> GetRandomListCollection(string category, string path, int howMany)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new CustomBasicException("Cannot have a blank category");
        }
        var arr = GetCustomList(category, path);
        if (arr!.HasValues == false)
        {
            return new BasicList<string>();
        }
        var list = Random.ListItems(arr, howMany);
        return list;
    }


    /// <summary>
    /// Picks a random BObject inside an array. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">key in the category</param>
    /// <returns>A random BObject based on the given path.</returns>
    protected internal virtual BObject? GetRandomBObject(string path)
    {
        var arr = GetCustomList(path);
        if (!arr!.HasValues)
        {
            return null;
        }
        return Random.ListItem(arr) as BObject;
    }
    /// <summary>
    /// Picks a random string inside a BSON array, then formats it. Only simple "." dotted JSON paths are supported.
    /// </summary>
    /// <param name="path">key in the category</param>
    /// <returns>A random formatted value.</returns>
    protected internal virtual string GetFormattedValue(string path)
    {
        var value = GetRandomListItem(path);

        var tokenResult = ParseTokens(value);

        return Random.Replace(tokenResult);
    }

    private static readonly Regex _parseTokensRegex = MyRegex();

    /// <summary>
    /// Recursive parse the tokens in the string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The parsed token.</returns>
    private string ParseTokens(string value)
    {
        var parseResult = _parseTokensRegex.Replace(
           value,
           x =>
           {
               BCustomList result;
               var groupValue = x.Groups[1].Value.ToLowerInvariant().Split('.');
               if (groupValue.Length == 1)
               {
                   result = (BCustomList)Database.Get(Category, groupValue[0])!;
               }
               else
               {
                   result = (BCustomList)Database.Get(groupValue[0], groupValue[1])!;
               }

               var randomElement = this.Random.ListItem(result);

               // replace values
               return ParseTokens(randomElement);
           });

        return parseResult;
    }

    [GeneratedRegex("\\#{(.*?)\\}", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
