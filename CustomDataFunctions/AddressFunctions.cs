namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class AddressFunctions<T>
    where T: Address
{
    public static Func<T, bool?, string> ZipCode { get; set; } = (obj, plusFour) =>
    {
        plusFour ??= obj.Random.Bool(30); //will be only 30 percent chance for plus 4
        return obj.CustomRandom.NextZipCode(plusFour.Value);
    };
    public static Func<T, int?, bool?, string> StreetName { get; set; } = (obj, syllables, shortSuffix) =>
    {
        shortSuffix ??= obj.Random.Bool(40);
        syllables ??= obj.Random.Int(1, 3);
        return obj.CustomRandom.NextStreet(syllables.Value, shortSuffix.Value); //i think.
    };
    //default is the same but can be changed.
    public static Func<T, int?, bool?, string> FullAddress { get; set; } = (obj, syllables, shortSuffix) =>
    {
        shortSuffix ??= obj.Random.Bool(40);
        syllables ??= obj.Random.Int(1, 3);
        return obj.CustomRandom.NextStreet(syllables.Value, shortSuffix.Value); //i think.
    };
    public static Func<T, string> CityPrefix { get; set; } = obj =>
    {
        return obj.GetRandomListItem("city_prefix");
    };
    public static Func<T, string> BuildingNumber { get; set; } = obj =>
    {
        return obj.GetRandomListItem("building_number");
    };
    public static Func<T, string> StreetSuffix { get; set; } = obj =>
    {
        return obj.GetRandomListItem("street_suffix");
    };
    public static Func<T, string> SecondaryAddress { get; set; } = obj =>
    {
        return obj.GetRandomListItem("secondary_address");
    };
}