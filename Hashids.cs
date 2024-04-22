namespace TestDataUSBasicLibrary;

/// <summary>
/// Generate YouTube-like hashes from one or many numbers. Use hashids when you do not want to expose your database ids to the user.
/// </summary>
public partial class Hashids : IHashids
{
    public const string DEFAULT_ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    public const string DEFAULT_SEPS = "cfhistuCFHISTU";
    private const double _sEP_DIV = 3.5;
    private const double _gUARD_DIV = 12.0;
    private string _alphabet;
    private readonly string _salt;
    private string _seps;
    private string? _guards;
    private readonly int _minHashLength;
    private Regex? _guardsRegex;
    private Regex? _sepsRegex;
    private static readonly Regex _hexValidator = MyRegex();
    private static readonly Regex _hexSplitter = MyRegex1();
    private static readonly char[] _separator = [' '];
    private static readonly char[] _separatorArray = [' '];

    /// <summary>
    /// Instantiates a new Hashids with the default setup.
    /// </summary>
    public Hashids() : this(string.Empty, 0, DEFAULT_ALPHABET, DEFAULT_SEPS)
    {
    }

    /// <summary>
    /// Instantiates a new Hashids en/de-coder.
    /// </summary>
    /// <param name="salt"></param>
    /// <param name="minHashLength"></param>
    /// <param name="alphabet"></param>
    public Hashids(string salt = "", int minHashLength = 0, string alphabet = DEFAULT_ALPHABET, string seps = DEFAULT_SEPS)
    {
        if (string.IsNullOrWhiteSpace(alphabet))
        {
            throw new ArgumentNullException(nameof(alphabet));
        }
        _salt = salt;
        _alphabet = string.Join(string.Empty, alphabet.Distinct());
        _seps = seps;
        _minHashLength = minHashLength;
        if (_alphabet.Length < 16)
        {
            throw new ArgumentException("alphabet must contain at least 4 unique characters.", nameof(alphabet));
        }
        SetupSeps();
        SetupGuards();
    }

    /// <summary>
    /// Encodes the provided numbers into a hashed string
    /// </summary>
    /// <param name="numbers">the numbers to encode</param>
    /// <returns>the hashed string</returns>
    public virtual string Encode(params int[] numbers)
    {
        return GenerateHashFrom(numbers.Select(n => (long)n).ToArray());
    }

    /// <summary>
    /// Encodes the provided numbers into a hashed string
    /// </summary>
    /// <param name="numbers">the numbers to encode</param>
    /// <returns>the hashed string</returns>
    public virtual string Encode(IEnumerable<int> numbers)
    {
        return Encode(numbers.ToArray());
    }

    /// <summary>
    /// Decodes the provided hash into
    /// </summary>
    /// <param name="hash">the hash</param>
    /// <exception cref="T:System.OverflowException">if the decoded number overflows integer</exception>
    /// <returns>the numbers</returns>
    public virtual int[] Decode(string hash)
    {
        return GetNumbersFrom(hash).Select(n => (int)n).ToArray();
    }

    /// <summary>
    /// Encodes the provided hex string to a hashids hash.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public virtual string EncodeHex(string hex)
    {
        if (!_hexValidator.IsMatch(hex))
        {
            return string.Empty;
        }
        var numbers = new List<long>();
        var matches = _hexSplitter.Matches(hex);
        foreach (Match match in matches.Cast<Match>())
        {
            var number = Convert.ToInt64(string.Concat("1", match.Value), 16);
            numbers.Add(number);
        }
        return EncodeLong([.. numbers]);
    }

    /// <summary>
    /// Decodes the provided hash into a hex-string
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public virtual string DecodeHex(string hash)
    {
        var ret = new StringBuilder();
        var numbers = DecodeLong(hash);
        foreach (var number in numbers)
        {
            ret.Append(string.Format("{0:X}", number).AsSpan(1));
        }
        return ret.ToString();
    }

    /// <summary>
    /// Decodes the provided hashed string into an array of longs 
    /// </summary>
    /// <param name="hash">the hashed string</param>
    /// <returns>the numbers</returns>
    public long[] DecodeLong(string hash)
    {
        return GetNumbersFrom(hash);
    }

    /// <summary>
    /// Encodes the provided longs to a hashed string
    /// </summary>
    /// <param name="numbers">the numbers</param>
    /// <returns>the hashed string</returns>
    public string EncodeLong(params long[] numbers)
    {
        return GenerateHashFrom(numbers);
    }

    /// <summary>
    /// Encodes the provided longs to a hashed string
    /// </summary>
    /// <param name="numbers">the numbers</param>
    /// <returns>the hashed string</returns>
    public string EncodeLong(IEnumerable<long> numbers)
    {
        return EncodeLong(numbers.ToArray());
    }
    private void SetupSeps()
    {
        // seps should contain only characters present in alphabet; 
        _seps = new string(_seps.Intersect([.. _alphabet]).ToArray());

        // alphabet should not contain seps.
        _alphabet = new string(_alphabet.Except([.. _seps]).ToArray());

        _seps = ConsistentShuffle(_seps, _salt);

        if (_seps.Length == 0 || _alphabet.Length / _seps.Length > _sEP_DIV)
        {
            var sepsLength = (int)Math.Ceiling(_alphabet.Length / _sEP_DIV);
            if (sepsLength == 1)
            {
                sepsLength = 2;
            }
            if (sepsLength > _seps.Length)
            {
                var diff = sepsLength - _seps.Length;
                _seps += _alphabet.Substring(0, diff);
                _alphabet = _alphabet.Substring(diff);
            }
            else
            {
                _seps = _seps.Substring(0, sepsLength);
            }
        }
        _sepsRegex = new Regex(string.Concat("[", _seps, "]"), RegexOptions.Compiled);
        _alphabet = ConsistentShuffle(_alphabet, _salt);
    }
    private void SetupGuards()
    {
        var guardCount = (int)Math.Ceiling(_alphabet.Length / _gUARD_DIV);
        if (_alphabet.Length < 3)
        {
            _guards = _seps.Substring(0, guardCount);
            _seps = _seps.Substring(guardCount);
        }
        else
        {
            _guards = _alphabet.Substring(0, guardCount);
            _alphabet = _alphabet.Substring(guardCount);
        }
        _guardsRegex = new Regex(string.Concat("[", _guards, "]"), RegexOptions.Compiled);
    }

    /// <summary>
    /// Internal function that does the work of creating the hash
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    private string GenerateHashFrom(long[] numbers)
    {
        if (numbers == null || numbers.Length == 0)
        {
            return string.Empty;
        }
        var ret = new StringBuilder();
        var alphabet = _alphabet;
        long numbersHashInt = 0;
        for (var i = 0; i < numbers.Length; i++)
        {
            numbersHashInt += (int)(numbers[i] % (i + 100));
        }
        var lottery = alphabet[(int)(numbersHashInt % alphabet.Length)];
        ret.Append(lottery);
        for (var i = 0; i < numbers.Length; i++)
        {
            var number = numbers[i];
            var buffer = lottery + _salt + alphabet;
            alphabet = ConsistentShuffle(alphabet, buffer.Substring(0, alphabet.Length));
            var last = Hash(number, alphabet);
            ret.Append(last);
            if (i + 1 < numbers.Length)
            {
                number %= last[0] + i;
                var sepsIndex = (int)number % _seps.Length;

                ret.Append(_seps[sepsIndex]);
            }
        }
        if (ret.Length < _minHashLength)
        {
            var guardIndex = (int)(numbersHashInt + ret[0]) % _guards!.Length;
            var guard = _guards[guardIndex];
            ret.Insert(0, guard);
            if (ret.Length < _minHashLength)
            {
                guardIndex = (int)(numbersHashInt + ret[2]) % _guards.Length;
                guard = _guards[guardIndex];
                ret.Append(guard);
            }
        }
        var halfLength = alphabet.Length / 2;
        while (ret.Length < _minHashLength)
        {
            alphabet = ConsistentShuffle(alphabet, alphabet);
            ret.Insert(0, alphabet.AsSpan(halfLength));
            ret.Append(alphabet.AsSpan(0, halfLength));
            var excess = ret.Length - _minHashLength;
            if (excess > 0)
            {
                ret.Remove(0, excess / 2);
                ret.Remove(_minHashLength, ret.Length - _minHashLength);
            }
        }
        return ret.ToString();
    }
    private static string Hash(long input, string alphabet)
    {
        var hash = new StringBuilder();
        do
        {
            hash.Insert(0, alphabet[(int)(input % alphabet.Length)]);
            input /= alphabet.Length;
        } while (input > 0);

        return hash.ToString();
    }
    private static long Unhash(string input, string alphabet)
    {
        long number = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var pos = alphabet.IndexOf(input[i]);
            number += (long)(pos * Math.Pow(alphabet.Length, input.Length - i - 1));
        }
        return number;
    }
    private long[] GetNumbersFrom(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return [];
        }
        var alphabet = new string(_alphabet.ToCharArray());
        var ret = new List<long>();
        int i = 0;
        var hashBreakdown = _guardsRegex!.Replace(hash, " ");
        var hashArray = hashBreakdown.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
        if (hashArray.Length == 3 || hashArray.Length == 2)
        {
            i = 1;
        }
        hashBreakdown = hashArray[i];
        if (hashBreakdown[0] != default(char))
        {
            var lottery = hashBreakdown[0];
            hashBreakdown = hashBreakdown.Substring(1);
            hashBreakdown = _sepsRegex!.Replace(hashBreakdown, " ");
            hashArray = hashBreakdown.Split(_separatorArray, StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < hashArray.Length; j++)
            {
                var subHash = hashArray[j];
                var buffer = lottery + _salt + alphabet;
                alphabet = ConsistentShuffle(alphabet, buffer.Substring(0, alphabet.Length));
                ret.Add(Unhash(subHash, alphabet));
            }
            if (EncodeLong([.. ret]) != hash)
            {
                ret.Clear();
            }
        }
        return [.. ret];
    }
    private static string ConsistentShuffle(string alphabet, string salt)
    {
        if (string.IsNullOrWhiteSpace(salt))
        {
            return alphabet;
        }
        int v, p, n, j;
        v = p = _ = 0;
        for (var i = alphabet.Length - 1; i > 0; i--, v++)
        {
            v %= salt.Length;
            p += n = salt[v];
            j = (n + v + p) % i;
            var temp = alphabet[j];
            alphabet = alphabet.Substring(0, j) + alphabet[i] + alphabet.Substring(j + 1);
            alphabet = alphabet.Substring(0, i) + temp + alphabet.Substring(i + 1);
        }
        return alphabet;
    }
    [GeneratedRegex("^[0-9a-fA-F]+$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"[\w\W]{1,12}", RegexOptions.Compiled)]
    private static partial Regex MyRegex1();
}