using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.VehicleFunctions<TestDataUSBasicLibrary.DataSets.Vehicle>; //only needed here.
namespace TestDataUSBasicLibrary.DataSets;
public class Vehicle : InternalDataSet
{
    protected override string Category => nameof(Vehicle);
    private static string StrictUpperCase => "ABCDEFGHJKLMNPRSTUVWXYZ";
    private static string StrictAlphaNumericUpperCase => Chars.Numbers + StrictUpperCase;

    /// <summary>
    /// Generate a vehicle identification number (VIN).
    /// </summary>
    /// <param name="strict">Limits the acceptable characters to alpha numeric uppercase except I, O and Q.</param>
    public string Vin(bool strict = false)
    {
        var sb = new StringBuilder();
        var allowedUpperCase = Chars.UpperCase;
        var allowedAlphaNumericChars = Chars.AlphaNumericUpperCase;
        if (strict)
        {
            allowedUpperCase = StrictUpperCase;
            allowedAlphaNumericChars = StrictAlphaNumericUpperCase;
        }
        sb.Append(Random.String2(10, allowedAlphaNumericChars));
        sb.Append(Random.String2(1, allowedUpperCase));
        sb.Append(Random.String2(1, allowedAlphaNumericChars));
        sb.Append(Random.Number(min: 10000, max: 99999));
        return sb.ToString();
    }

    /// <summary>
    /// Get a vehicle manufacture name. IE: Toyota, Ford, Porsche.
    /// </summary>
    public string Manufacturer()
    {
        return ee1.Manufacturer(this);
    }

    /// <summary>
    /// Get a vehicle model. IE: Camry, Civic, Accord.
    /// </summary>
    public string Model()
    {
        return ee1.Model(this);
    }

    /// <summary>
    /// Get a vehicle type. IE: Minivan, SUV, Sedan.
    /// </summary>
    public string Type()
    {
        return ee1.Type(this);
    }

    /// <summary>
    /// Get a vehicle fuel type. IE: Electric, Gasoline, Diesel.
    /// </summary>
    public string Fuel()
    {
        return GetRandomListItem("fuel");
    }
}