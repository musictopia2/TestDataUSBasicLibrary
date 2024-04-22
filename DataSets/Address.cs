using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.AddressFunctions<TestDataUSBasicLibrary.DataSets.Address>;
namespace TestDataUSBasicLibrary.DataSets;
public class Address : InternalDataSet
{
    protected override string Category => nameof(Address);
    internal static IRandomData CustomData => rr3.GetRandomDataClass();
    internal IRandomNumberList CustomRandom => Random;
    /// <summary>
    /// Get a zipcode.
    /// </summary>
    /// <param name="plusFour">
    /// if set to true, then would be the - and the 4 extra characters
    /// </param>
    /// <returns>A random zipcode.</returns>
    public string ZipCode(bool? plusFour = null)
    {
        return ee1.ZipCode(this, plusFour);
    }
    /// <summary>
    /// Get a street name.
    /// </summary>
    /// <param name="syllables">
    /// the number to use.  if not specified, then will pick random from 1 to 3
    /// </param>
    /// <param name="shortSuffix">
    /// if not specified, then has a 40 percent chance of being it
    /// </param>
    /// <returns>A random street name.</returns>
    public string StreetName(int? syllables = null, bool? shortSuffix = null)
    {
        return ee1.StreetName(this, syllables, shortSuffix);
    }
    /// <summary>
    /// returns a full address
    /// </summary>
    /// <param name="syllables">
    /// the number to use.  if not specified, then will pick random from 1 to 3
    /// </param>
    /// <param name="shortSuffix">
    /// if not specified, then has a 40 percent chance of being it
    /// </param>
    /// <returns></returns>
    public string FullAddress(int? syllables = null, bool? shortSuffix = null)
    {
        return ee1.FullAddress(this, syllables, shortSuffix);
    }
    /// <summary>
    /// Get a city prefix.
    /// </summary>
    /// <returns>A random city prefix.</returns>
    public string CityPrefix()
    {
        return ee1.CityPrefix(this);
    }
    /// <summary>
    /// Get a building number.
    /// </summary>
    /// <returns>A random building number.</returns>
    public string BuildingNumber()
    {
        return ee1.BuildingNumber(this);
    }
    /// <summary>
    /// Get a street suffix.
    /// </summary>
    /// <returns>A random street suffix.</returns>
    public string StreetSuffix()
    {
        return ee1.StreetSuffix(this);
    }
    /// <summary>
    /// Get a secondary address like 'Apt. 2' or 'Suite 321'.
    /// </summary>
    /// <returns>A random secondary address.</returns>
    public string SecondaryAddress()
    {
        return ee1.SecondaryAddress(this);
    }
    /// <summary>
    /// Get a random state state.
    /// </summary>
    /// <returns>A random state.</returns>
    public string State(string city = "")
    {
        if (city == "")
        {
            return GetRandomListItem("state");
        }
        //if the city chosen does not exist, then raise error.
        return "";
    }

    /// <summary>
    /// Get a state abbreviation.
    /// </summary>
    /// <returns>An abbreviation for a random state.</returns>
    public string StateAbbr(string city = "")
    {
        if (city == "")
        {
            return GetRandomListItem("state_abbr");
        }
        //we don't have anything yet for city/state combinations.
        return "";
    }
    /// <summary>
    /// Get a Latitude.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>A random latitude value.</returns>
    public double Latitude(double min = -90, double max = 90)
    {
        return Math.Round(Random.Double(min, max), 4);
    }

    /// <summary>
    /// Get a Longitude.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>A random longitude value.</returns>
    public double Longitude(double min = -180, double max = 180)
    {
        return Math.Round(Random.Double(min, max), 4);
    }

    /// <summary>
    /// Generates a cardinal or ordinal direction. IE: Northwest, South, SW, E.
    /// </summary>
    /// <param name="useAbbreviation">When true, directions such as Northwest turn into NW.</param>
    /// <returns>A random cardinal or ordinal direction.</returns>
    public string Direction(bool useAbbreviation = false)
    {
        if (useAbbreviation)
        {
            return GetRandomListItem("direction_abbr");
        }

        return GetRandomListItem("direction");
    }

    /// <summary>
    /// Generates a cardinal direction. IE: North, South, E, W.
    /// </summary>
    /// <param name="useAbbreviation">When true, directions such as West turn into W.</param>
    /// <returns>A random cardinal direction</returns>
    public string CardinalDirection(bool useAbbreviation = false)
    {
        if (useAbbreviation)
        {
            return GetRandomListItem("direction_abbr", min: 0, max: 4);
        }

        return GetRandomListItem("direction", min: 0, max: 4);
    }

    /// <summary>
    /// Generates an ordinal direction. IE: Northwest, Southeast, SW, NE.
    /// </summary>
    /// <param name="useAbbreviation">When true, directions such as Northwest turn into NW.</param>
    /// <returns>A random ordinal direction.</returns>
    public string OrdinalDirection(bool useAbbreviation = false)
    {
        if (useAbbreviation)
        {
            return GetRandomListItem("direction_abbr", min: 4, max: 8);
        }

        return GetRandomListItem("direction", min: 4, max: 8);
    }
}