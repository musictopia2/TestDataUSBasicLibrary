using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.FinanceFunctions<TestDataUSBasicLibrary.DataSets.Finance>;
namespace TestDataUSBasicLibrary.DataSets;
public class Finance : InternalDataSet
{
    protected override string Category => nameof(Finance);
    /// <summary>
    /// Get an account number. Default length is 8 digits.
    /// </summary>
    /// <param name="length">The length of the account number.</param>
    public string AccountNumber(int length = 8)
    {
        var template = new string('#', length);
        return Random.Replace(template);
    }
    /// <summary>
    /// Get an account name. Like "savings", "checking", "Home Loan" etc..
    /// </summary>
    public string AccountType()
    {
        return ee1.AccountType(this);
    }
    /// <summary>
    /// Get a random amount. Default 0 - 1000.
    /// </summary>
    /// <param name="min">Min value. Default 0.</param>
    /// <param name="max">Max value. Default 1000.</param>
    /// <param name="decimals">Decimal places. Default 2.</param>
    public decimal Amount(decimal min = 0, decimal max = 1000, int decimals = 2)
    {
        var amount = (max - min);
        var part = (decimal)Random.Double() * amount;
        return Math.Round(min + part, decimals);
    }


    /// <summary>
    /// Get a transaction type: "deposit", "withdrawal", "payment", or "invoice".
    /// </summary>
    public string TransactionType()
    {
        return ee1.TransactionType(this);
    }
    /// <summary>
    /// Generate a random credit card number with valid Luhn checksum.
    /// </summary>
    /// <param name="cardType">This is the credit card company to use like American Express</param>
    public string CreditCardNumber(string? cardType = null)
    {
        return ee1.CreditCardNumber(this, cardType);
    }
    /// <summary>
    /// Generate a credit card CVV.
    /// </summary>
    public string CreditCardCvv()
    {
        return Random.Replace("###");
    }
    private static BasicList<char> BtcCharset =>
        [
         '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
         'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
         'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        ];


    /// <summary>
    /// Generates a random Bitcoin address.
    /// </summary>
    public string BitcoinAddress()
    {
        var addressLength = Random.Number(25, 34);
        IBasicList<char> lastBits = Random.ListItems(BtcCharset, addressLength);
        string fins = lastBits.GetString();
        if (Random.Bool())
        {
            return $"1{fins}";
        }
        return $"3{fins}";
    }

    /// <summary>
    /// Generate a random Ethereum address.
    /// </summary>
    public string EthereumAddress()
    {
        return Random.Hexadecimal(40);
    }

    /// <summary>
    /// Generate a random Litecoin address.
    /// </summary>
    public string LitecoinAddress()
    {
        var addressLength = Random.Number(26, 33);
        IBasicList<char> lastBits = Random.ListItems(BtcCharset, addressLength);
        string fins = lastBits.GetString();
        var prefix = Random.Number(0, 2);

        if (prefix == 0)
        {
            return $"L{fins}";
        }
        if (prefix == 1)
        {
            return $"M{fins}";
        }
        return $"3{fins}";
    }

    /// <summary>
    /// Generates an ABA routing number with valid check digit.
    /// </summary>
    public string RoutingNumber()
    {
        var digits = Random.Digits(8);

        var sum = 0;
        for (var i = 0; i < digits.Count; i += 3)
        {
            sum += 3 * digits.ElementAt(i);
            sum += 7 * digits.ElementAt(i + 1);
            sum += digits.ElementAtOrDefault(i + 2);
        }

        var checkDigit = Math.Ceiling(sum / 10d) * 10 - sum;

        return digits.Aggregate("", (str, digit) => str + digit, str => str + checkDigit);
    }

    protected static readonly BasicList<string> BicVowels = ["A", "E", "I", "O", "U"];

    /// <summary>
    /// Generates Bank Identifier Code (BIC) code.
    /// </summary>
    public string Bic()
    {
        var prob = Random.Number(100);
        return Random.Replace("???") +
               Random.ListItem(BicVowels) +
               "US" +
               Random.Replace("?") + "1" +
               (prob < 10 ? Random.Replace("?" + Random.ListItem(BicVowels) + "?") : prob < 40 ? Random.Replace("###") : "");
    }
}