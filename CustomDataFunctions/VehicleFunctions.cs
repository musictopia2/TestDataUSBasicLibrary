namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class VehicleFunctions<T> where T: Vehicle
{
    public static Func<T, string> Manufacturer { get; set; } = vehicle =>
    {
        return vehicle.GetRandomListItem("manufacturer");
    };
    public static Func<T, string> Model { get; set; } = vehicle =>
    {
        return vehicle.GetRandomListItem("model");
    };
    public static Func<T, string> Type { get; set; } = vehicle =>
    {
        return vehicle.GetRandomListItem("type");
    };
}