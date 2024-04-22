﻿namespace TestDataUSBasicLibrary;
/// <summary>
/// A randomizer that randomizes things.
/// </summary>
public class Randomizer : IRandomNumberList, IRandomizer
{
    //bogus just used the built in random.  i will follow that sample.

    /// <summary>
    /// Set the random number generator manually with a seed to get reproducible results.
    /// </summary>
    public static Random Seed { get; set; } = new();
    public int Test { get; set; }
    internal Lazy<object> _localLocker;
    internal static Lazy<object> _locker = new(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// The pseudo-random number generator that is used for all random number generation in this instance.
    /// </summary>
    protected Random LocalSeed { get; set; }
    /// <summary>
    /// Constructor that uses the global static `<see cref="Seed"/>.
    /// Changing the global static seed after this constructor runs
    /// will have no effect. A new randomizer is needed to capture a new
    /// global seed.
    /// </summary>
    public Randomizer()
    {
        LocalSeed = Seed;
        _localLocker = _locker;
    }
    /// <summary>
    /// Constructor that uses <see cref="LocalSeed"/> parameter as a seed.
    /// Completely ignores the global static <see cref="Seed"/>.
    /// </summary>
    public Randomizer(int localSeed)
    {
        LocalSeed = new Random(localSeed);
        _localLocker = new(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);
    }
    //could try to have sharing for the logic i have for my custom random stuff (even more than i currently have).
    BasicList<int> IRandomNumberList.GenerateRandomList(int maxNumber, int howMany, int startingNumber, BasicList<int>? previousList, BasicList<int>? setToContinue, bool putBefore)
    {
        //lock any seed access, for thread safety.
        lock (_localLocker.Value)
        {
            return this.GenerateRandomList(maxNumber, howMany, startingNumber, previousList, setToContinue, putBefore);
        }
    }
    internal int Next(int max)
    {
        return (int)Math.Floor(LocalSeed.NextDouble() * (max));
    }
    internal int Next(int min, int max)
    {
        return (int)Math.Floor(LocalSeed.NextDouble() * (max - min) + min);
    }
    int IRandomNumberList.GetRandomNumber(int maxNumber, int startingPoint, BasicList<int>? previousList)
    {
        //lock any seed access, for thread safety.
        lock (_localLocker.Value)
        {
            return this.GetRandomNumber(maxNumber, startingPoint, previousList);
        }
    }


    /// <summary>
    /// Generate a random byte between 0 and 255.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default byte.MinValue 0</param>
    /// <param name="max">Max value, inclusive. Default byte.MaxValue 255</param>
    public byte Byte(byte min = byte.MinValue, byte max = byte.MaxValue)
    {
        return Convert.ToByte(Number(min, max));
    }

    /// <summary>
    /// Get a random sequence of bytes.
    /// </summary>
    /// <param name="count">The size of the byte array</param>
    public byte[] Bytes(int count)
    {
        var arr = new byte[count];
        lock (_localLocker.Value)
        {
            LocalSeed.NextBytes(arr);
        }
        return arr;
    }


    /// <summary>
    /// Get an int from 1 to max.
    /// </summary>
    /// <param name="max">Upper bound, inclusive.</param>
    public int Number(int max)
    {
        return Number(1, max);
    }


    /// <summary>
    /// Get an int from min to max.
    /// </summary>
    /// <param name="min">Lower bound, inclusive</param>
    /// <param name="max">Upper bound, inclusive</param>
    public int Number(int min = 0, int max = 1)
    {
        //lock any seed access, for thread safety.
        lock (_localLocker.Value)
        {
            // Adjust the range as needed to make max inclusive. The Random.Next function uses exclusive upper bounds.

            // If max can be extended by 1, just do that.
            if (max < int.MaxValue)
            {
                return LocalSeed.Next(min, max + 1);
            }

            // If max is exactly int.MaxValue, then check if min can be used to push the range out by one the other way.
            // If so, then we can simply add one to the result to put it back in the correct range.
            if (min > int.MinValue)
            {
                return 1 + LocalSeed.Next(min - 1, max);
            }

            // If we hit this line, then min is int.MinValue and max is int.MaxValue, which mean the caller wants a
            // number from a range spanning all possible values of int. The Random class only supports exclusive
            // upper bounds, period, and the upper bound must be specified as an int, so the best we can get in a
            // single call is a value in the range (int.MinValue, int.MaxValue - 1). Instead, what we do is get two
            // samples, each of which has just under 31 bits of entropy, and use 16 bits from each to assemble a
            // single 16-bit number.
            int sample1 = LocalSeed.Next();
            int sample2 = LocalSeed.Next();

            int topHalf = (sample1 >> 8) & 0xFFFF;
            int bottomHalf = (sample2 >> 8) & 0xFFFF;

            return unchecked((topHalf << 16) | bottomHalf);
        }
    }
    /// <summary>
    /// Get a random sequence of digits.
    /// </summary>
    /// <param name="count">How many</param>
    /// <param name="minDigit">minimum digit, inclusive</param>
    /// <param name="maxDigit">maximum digit, inclusive</param>
    public BasicList<int> Digits(int count, int minDigit = 0, int maxDigit = 9)
    {
        BasicList<int> digits = [];
        if (maxDigit > 9 || maxDigit < 0)
        {
            throw new ArgumentException("max digit can't be lager than 9 or smaller than 0", nameof(maxDigit));
        }
        if (minDigit > 9 || minDigit < 0)
        {
            throw new ArgumentException("min digit can't be lager than 9 or smaller than 0", nameof(minDigit));
        }
        for (var i = 0; i < count; i++)
        {
            int x = Number(min: minDigit, max: maxDigit);
            digits.Add(x);
        }
        return digits;
    }
    /// <summary>
    /// Returns a random even number. If the range does not contain any even numbers, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="min">Lower bound, inclusive</param>
    /// <param name="max">Upper bound, inclusive</param>
    /// <exception cref="ArgumentException">Thrown if it is impossible to select an odd number satisfying the specified range.</exception>
    public int Even(int min = 0, int max = 1)
    {
        // Ensure that we have a valid range.
        if (min > max)
        {
            throw new ArgumentException($"The min/max range is invalid. The minimum value '{min}' is greater than the maximum value '{max}'.", nameof(max));
        }
        if (((min & 1) == 1) && (max - 1 < min))
        {
            throw new ArgumentException("The specified range does not contain any even numbers.", nameof(max));
        }

        // Adjust the range to ensure that we always get the same number of even values as odd values.
        // For example,
        //   if the input is min = 1, max = 3, the new range should be min = 2, max = 3.
        //   if the input is min = 2, max = 3, the range should remain min = 2, max = 3.
        min = (min + 1) & ~1;
        max |= 1;
        if (min > max)
        {
            return min;
        }

        // Strip off the last bit of a random number to make the number even.
        return Number(min, max) & ~1;
    }
    /// <summary>
    /// Returns a random odd number. If the range does not contain any odd numbers, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="min">Lower bound, inclusive</param>
    /// <param name="max">Upper bound, inclusive</param>
    /// <exception cref="ArgumentException">Thrown if it is impossible to select an odd number satisfying the specified range.</exception>
    public int Odd(int min = 0, int max = 1)
    {
        // Ensure that we have a valid range.
        if (min > max)
        {
            throw new ArgumentException($"The min/max range is invalid. The minimum value '{min}' is greater than the maximum value '{max}'.", nameof(max));
        }

        if (((max & 1) == 0) && (min + 1 > max))
        {
            throw new ArgumentException("The specified range does not contain any odd numbers.", nameof(max));
        }

        // Special case where the math below breaks.
        if (max == int.MinValue)
        {
            return int.MinValue | 1;
        }

        // Adjust the range to ensure that we always get the same number of even values as odd values.
        // For example,
        //   if the input is min = 2, max = 4, the new range should be min = 2, max = 3.
        //   if the input is min = 2, max = 3, the range should remain min = 2, max = 3.
        min &= ~1;
        max = (max - 1) | 1;

        if (min > max)
        {
            return min | 1;
        }

        // Ensure that the last bit is set in a random number to make the number odd.
        return Number(min, max) | 1;
    }
    /// <summary>
    /// Get a random double, between 0.0 and 1.0.
    /// </summary>
    /// <param name="min">Minimum, inclusive. Default 0.0</param>
    /// <param name="max">Maximum, exclusive. Default 1.0</param>
    public double Double(double min = 0.0d, double max = 1.0d)
    {
        //lock any seed access, for thread safety.
        lock (_localLocker.Value)
        {
            if (min == 0.0d && max == 1.0d)
            {
                //use default implementation
                return LocalSeed.NextDouble();
            }
            return LocalSeed.NextDouble() * (max - min) + min;
        }
    }

    /// <summary>
    /// Get a random decimal, between 0.0 and 1.0.
    /// </summary>
    /// <param name="min">Minimum, inclusive. Default 0.0</param>
    /// <param name="max">Maximum, exclusive. Default 1.0</param>
    public decimal Decimal(decimal min = 0.0m, decimal max = 1.0m)
    {
        return Convert.ToDecimal(Double()) * (max - min) + min;
    }

    /// <summary>
    /// Get a random float, between 0.0 and 1.0.
    /// </summary>
    /// <param name="min">Minimum, inclusive. Default 0.0</param>
    /// <param name="max">Maximum, inclusive. Default 1.0</param>
    public float Float(float min = 0.0f, float max = 1.0f)
    {
        return Convert.ToSingle(Double() * (max - min) + min);
    }
    /// <summary>
    /// Generate a random int between MinValue and MaxValue.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default int.MinValue</param>
    /// <param name="max">Max value, inclusive. Default int.MaxValue</param>
    public int Int(int min = int.MinValue, int max = int.MaxValue)
    {
        return Number(min, max);
    }
    /// <summary>
    /// Generate a random long between MinValue and MaxValue.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default long.MinValue</param>
    /// <param name="max">Max value, inclusive. Default long.MaxValue</param>
    public long Long(long min = long.MinValue, long max = long.MaxValue)
    {
        var range = (decimal)max - min; //use more bits?
        return Convert.ToInt64((decimal)Double() * range + min);
    }
    /// <summary>
    /// Generate a random short between MinValue and MaxValue.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default short.MinValue -32768</param>
    /// <param name="max">Max value, inclusive. Default short.MaxValue 32767</param>
    public short Short(short min = short.MinValue, short max = short.MaxValue)
    {
        return Convert.ToInt16(Double() * (max - min) + min);
    }
    /// <summary>
    /// Generate a random char between MinValue and MaxValue.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default char.MinValue</param>
    /// <param name="max">Max value, inclusive. Default char.MaxValue</param>
    public char Char(char min = char.MinValue, char max = char.MaxValue)
    {
        return Convert.ToChar(Number(min, max));
    }
    /// <summary>
    /// Generate a random chars between MinValue and MaxValue.
    /// </summary>
    /// <param name="min">Min value, inclusive. Default char.MinValue</param>
    /// <param name="max">Max value, inclusive. Default char.MaxValue</param>
    /// <param name="count">The length of chars to return</param>
    public BasicList<char> Chars(char min = char.MinValue, char max = char.MaxValue, int count = 5)
    {
        BasicList<char> output = [];
        //var arr = new char[count];
        for (var i = 0; i < count; i++)
        {
            output.Add(Char(min, max));
        }
        return output;
    }
    /// <summary>
    /// Get a string of characters of a specific length.
    /// Uses <seealso cref="Chars"/>.
    /// Note: This method can return ill-formed UTF16 Unicode strings with unpaired surrogates.
    /// Use <seealso cref="Utf16String"/> for technically valid Unicode.
    /// </summary>
    /// <param name="length">The exact length of the result string. If null, a random length is chosen between 40 and 80.</param>
    /// <param name="minChar">Min character value, inclusive. Default char.MinValue</param>
    /// <param name="maxChar">Max character value, inclusive. Default char.MaxValue</param>
    public string String(int? length = null, char minChar = char.MinValue, char maxChar = char.MaxValue)
    {
        var l = length ?? Number(40, 80);
        var list = Chars(minChar, maxChar, l);
        var array = list.ToArray();
        return new string(array);
    }
    /// <summary>
    /// Get a string of characters between <paramref name="minLength" /> and <paramref name="maxLength"/>.
    /// Uses <seealso cref="Chars"/>.
    /// Note: This method can return ill-formed UTF16 Unicode strings with unpaired surrogates.
    /// Use <seealso cref="Utf16String"/> for technically valid Unicode.
    /// </summary>
    /// <param name="minLength">Lower-bound string length. Inclusive.</param>
    /// <param name="maxLength">Upper-bound string length. Inclusive.</param>
    /// <param name="minChar">Min character value, inclusive. Default char.MinValue</param>
    /// <param name="maxChar">Max character value, inclusive. Default char.MaxValue</param>
    public string String(int minLength, int maxLength, char minChar = char.MinValue, char maxChar = char.MaxValue)
    {
        var length = Number(minLength, maxLength);
        return String(length, minChar, maxChar);
    }
    /// <summary>
    /// Get a string of characters with a specific length drawing characters from <paramref name="chars"/>.
    /// The returned string may contain repeating characters from the <paramref name="chars"/> string.
    /// </summary>
    /// <param name="length">The length of the string to return.</param>
    /// <param name="chars">The pool of characters to draw from. The returned string may contain repeat characters from the pool.</param>
    public string String2(int length, string chars = "abcdefghijklmnopqrstuvwxyz")
    {
        var target = new char[length];
        for (int i = 0; i < length; i++)
        {
            var idx = Number(0, chars.Length - 1);
            target[i] = chars[idx];
        }
        return new string(target);
    }
    /// <summary>
    /// Get a string of characters with a specific length drawing characters from <paramref name="chars"/>.
    /// The returned string may contain repeating characters from the <paramref name="chars"/> string.
    /// </summary>
    /// <param name="minLength">The minimum length of the string to return, inclusive.</param>
    /// <param name="maxLength">The maximum length of the string to return, inclusive.</param>
    /// <param name="chars">The pool of characters to draw from. The returned string may contain repeat characters from the pool.</param>
    public string String2(int minLength, int maxLength, string chars = "abcdefghijklmnopqrstuvwxyz")
    {
        var length = Number(minLength, maxLength);
        return String2(length, chars);
    }
    /// <summary>
    /// Return a random hex hash. Default 40 characters, aka SHA-1.
    /// </summary>
    /// <param name="length">The length of the hash string. Default, 40 characters, aka SHA-1.</param>
    /// <param name="upperCase">Returns the hex string with uppercase characters.</param>
    public string Hash(int length = 40, bool upperCase = false)
    {
        return String2(length, upperCase ? TestDataUSBasicLibrary.Chars.HexUpperCase : TestDataUSBasicLibrary.Chars.HexLowerCase);
    }
    /// <summary>
    /// Get a random boolean.
    /// </summary>
    public bool Bool()
    {
        return Bool(50);
    }
    /// <summary>
    /// Get a random boolean.
    /// </summary>
    /// <param name="likelihood">The probability of true. 50 means equal possibility.</param>
    public bool Bool(int likelihood)
    {
        double item = Double() * 100;
        if (item <= likelihood)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Get a random list item.
    /// </summary>
    public T ListItem<T>(BasicList<T> list)
    {
        list.SendRandoms(this); //hopefully will work (?)
        return list.GetRandomItem();
    }

    public IBasicList<string> ListItems(BCustomList props, int howMany)
    {
        BasicList<string> output = [];
        foreach (var item in props.FullList)
        {
            output.Add(item); //try this way (?)
        }
        output.SendRandoms(this);
        return output.GetRandomList(false, howMany);
    }

    /// <summary>
    /// Helper method to get a random element in a BSON custom list.
    /// </summary>
    public BValue ListItem(BCustomList props, int? min = null, int? max = null)
    {
        var r = Number(min: min ?? 0, max: max - 1 ?? props.Count - 1);
        return props[r];
    }
    /// <summary>
    /// Get a random subset of a List.
    /// </summary>
    /// <param name="items">The source of items to pick from.</param>
    /// <param name="count">The number of items to pick; otherwise, a random amount is picked.</param>
    public IBasicList<T> ListItems<T>(BasicList<T> items, int? count = null)
    {
        if (count > items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        count ??= Number(0, items.Count - 1);
        items.SendRandoms(this);
        var output = items.GetRandomList(false, count.Value);
        return output;
    }
    /// <summary>
    /// Replaces symbols with numbers.
    /// IE: ### -> 283
    /// </summary>
    /// <param name="format">The string format</param>
    /// <param name="symbol">The symbol to search for in format that will be replaced with a number</param>
    public string ReplaceNumbers(string format, char symbol = '#')
    {
        return ReplaceSymbols(format, symbol, () => Convert.ToChar('0' + Number(9)));
    }

    /// <summary>
    /// Replaces each character instance in a string.
    /// Func is called each time a symbol is encountered.
    /// </summary>
    /// <param name="format">The string with symbols to replace.</param>
    /// <param name="symbol">The symbol to search for in the string.</param>
    /// <param name="func">The function that produces a character for replacement. Invoked each time the replacement symbol is encountered.</param>
    public static string ReplaceSymbols(string format, char symbol, Func<char> func)
    {
        var chars = format.Select(c => c == symbol ? func() : c).ToArray();
        return new string(chars);
    }

    /// <summary>
    /// Replaces symbols with numbers and letters. # = number, ? = letter, * = number or letter.
    /// IE: ###???* -> 283QED4. Letters are uppercase.
    /// </summary>
    public string Replace(string format)
    {
        var chars = format.Select(c =>
        {
            if (c == '*')
            {
                c = Bool() ? '#' : '?';
            }
            if (c == '#')
            {
                return Convert.ToChar('0' + Number(9));
            }
            if (c == '?')
            {
                return Convert.ToChar('A' + Number(25));
            }

            return c;
        })
           .ToArray();
        return new string(chars);
    }

    /// <summary>
    /// Clamps the length of a string between min and max characters.
    /// If the string is below the minimum, the string is appended with random characters up to the minimum length.
    /// If the string is over the maximum, the string is truncated at maximum characters; additionally, if the result string ends with
    /// whitespace, it is replaced with a random characters.
    /// </summary>
    public string ClampString(string str, int? min = null, int? max = null)
    {
        if (max != null && str.Length > max)
        {
            str = str.Substring(0, max.Value).Trim();
        }
        if (min != null && min > str.Length)
        {
            var missingChars = min - str.Length;
            var fillerChars = Replace("".PadRight(missingChars.Value, '?'));
            return str + fillerChars;
        }
        return str;
    }
    /// <summary>
    /// Get a random GUID.
    /// </summary>
    public Guid Guid()
    {
        string firstString = this.NextGUID();
        Guid guid = new(firstString);
        return guid; //hopefully this works.
    }

    private readonly static BasicList<char> _alphaChars =
        [
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
         'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
         'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
         'u', 'v', 'w', 'x', 'y', 'z'
        ];



    /// <summary>
    /// Returns a random set of alpha numeric characters 0-9, a-z.
    /// </summary>
    public string AlphaNumeric(int length)
    {
        var sb = new StringBuilder();
        return Enumerable.Range(1, length).Aggregate(sb, (b, i) => b.Append(ListItem(_alphaChars)), b => b.ToString());
    }
    private readonly static BasicList<char> _hexChars =
        [
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
         'a', 'b', 'c', 'd', 'e', 'f'
        ];


    /// <summary>
    /// Generates a random hexadecimal string.
    /// </summary>
    public string Hexadecimal(int length = 1, string prefix = "0x")
    {
        var sb = new StringBuilder();
        return Enumerable.Range(1, length).Aggregate(sb, (b, i) => b.Append(ListItem(_hexChars)), b => $"{prefix}{b}");
    }
    //items are weighted by the decimal probability in their value
    /// <summary>
    /// Returns a selection of T[] based on a weighted distribution of probability.
    /// </summary>
    /// <param name="items">Items to draw the selection from.</param>
    /// <param name="weights">Weights in decimal form: IE:[.25, .50, .25] for total of 3 items. Should add up to 1.</param>
    public T AddWeightedItem<T>(BasicList<T> items, BasicList<float> weights)
    {
        if (weights.Count != items.Count)
        {
            throw new ArgumentOutOfRangeException($"{nameof(items)}.Length and {nameof(weights)}.Length must be the same.");
        }
        var rand = Float();
        float max;
        float min = 0f;
        var item = default(T);
        for (int i = 0; i < weights.Count; i++)
        {
            max = min + weights[i];
            item = items[i];
            if (rand >= min && rand <= max)
            {
                break;
            }
            min += weights[i];
        }
        return item!;
    }
    int IRandomNumberList.Next(int max)
    {
        return AltNext(max: max);
    }
    int IRandomNumberList.Next(int min, int max)
    {
        return AltNext(min, max);
    }
    private int AltNext(int max)
    {
        lock (_localLocker.Value)
        {
            return (int)Math.Floor(LocalSeed.NextDouble() * max);
        }
    }
    private int AltNext(int min, int max)
    {
        lock (_localLocker.Value)
        {
            return (int)Math.Floor(LocalSeed.NextDouble() * (max - min) + min);
        }       
    }
}