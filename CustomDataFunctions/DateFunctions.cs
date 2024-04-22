namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class DateFunctions<T>
    where T : Date
{
    public static Func<T, string> TimeZoneString { get; set; } = obj =>
    {
        BasicList<string> possibleTimeZones =
            [
                "Pacific",
                "Mountain-Arizona",
                "Mountain-Standard",
                "Central",
                "Eastern"
            ];
        return obj.Random.ListItem(possibleTimeZones); //should use the random version so it properly sets the randoms and makes it deterministic.
    };
}