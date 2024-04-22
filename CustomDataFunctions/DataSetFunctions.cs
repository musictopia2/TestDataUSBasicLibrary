namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class DataSetFunctions<T>
    where T : DataSet
{
    public static Func<T, string> Color { get; set; } = obj =>
    {
        var data = DataSet.GetCustomData;
        var list = data.ColorNames;
        return obj.Random.ListItem(list);
    };
}