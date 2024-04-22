using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.CommerceFunctions<TestDataUSBasicLibrary.DataSets.Commerce>;
namespace TestDataUSBasicLibrary.DataSets;
public class Commerce : InternalDataSet
{
    protected override string Category => nameof(Commerce);
    /// <summary>
    /// Get a random commerce department.
    /// </summary>
    /// <param name="max">The maximum amount of departments</param>
    /// <param name="returnMax">If true the method returns the max amount of values, otherwise the number of categories returned is between 1 and max.</param>
    /// <returns>A random commerce department.</returns>
    public string Department(int max = 3, bool returnMax = false)
    {
        return ee1.Department(this, max, returnMax);
    }

    // there is an easier way to do this.
    // check finance.amount
    /// <summary>
    /// Get a random product price.
    /// </summary>
    /// <param name="min">The minimum price.</param>
    /// <param name="max">The maximum price.</param>
    /// <param name="decimals">How many decimals the number may include.</param>
    /// <param name="symbol">The symbol in front of the price.</param>
    /// <returns>A randomly generated price.</returns>
    public string Price(decimal min = 1, decimal max = 1000, int decimals = 2, string symbol = "")
    {
        var amount = max - min;
        var part = (decimal)Random.Double() * amount;
        return symbol + Math.Round(min + part, decimals);
    }

    /// <summary>
    /// Get random product categories.
    /// </summary>
    /// <param name="num">The amount of categories to be generated.</param>
    /// <returns>A collection of random product categories.</returns>
    public IBasicList<string> Categories(int num)
    {
        return ee1.Categories(this, num);
    }

    /// <summary>
    /// Get a random product name.
    /// </summary>
    /// <returns>A random product name.</returns>
    public string ProductName()
    {
        return ee1.ProductName(this);
    }

    

    /// <summary>
    /// Get a random product.
    /// </summary>
    /// <returns>A random product.</returns>
    public string Product()
    {
        return ee1.Product(this);
    }

    /// <summary>
    /// Random product adjective.
    /// </summary>
    /// <returns>A random product adjective.</returns>
    public string ProductAdjective()
    {
        return ee1.ProductAdjective(this);
    }

    /// <summary>
    /// Random product material.
    /// </summary>
    /// <returns>A random product material.</returns>
    public string ProductMaterial()
    {
        return ee1.ProductMaterial(this);
    }

    

    /// <summary>
    /// EAN-8 checksum weights.
    /// </summary>
    protected static BasicList<int> Ean8Weights => [ 3, 1, 3, 1, 3, 1, 3 ];

    /// <summary>
    /// Get a random EAN-8 barcode number.
    /// </summary>
    /// <returns>A random EAN-8 barcode number.</returns>
    public string Ean8()
    {
        // [3, 1, 3, 1, 3, 1, 3]
        return Ean(8, Ean8Weights);
    }

    /// <summary>
    /// EAN-18 checksum weights.
    /// </summary>
    protected static BasicList<int> Ean13Weights => [ 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3 ];

    /// <summary>
    /// Get a random EAN-13 barcode number.
    /// </summary>
    /// <returns>A random EAN-13 barcode number.</returns>
    public string Ean13()
    {
        // [1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3]
        return Ean(13, Ean13Weights);
    }
    private string Ean(int length, BasicList<int> weights)
    {
        var digits = Random.Digits(length - 1);
        var weightedSum =
           digits.Zip(weights,
                 (d, w) => d * w)
              .Sum();

        var checkDigit = (10 - weightedSum % 10) % 10;

        return string.Join("", digits) + checkDigit;
    }
}