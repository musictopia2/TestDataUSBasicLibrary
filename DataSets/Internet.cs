using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.InternetFunctions<TestDataUSBasicLibrary.DataSets.Internet>;
namespace TestDataUSBasicLibrary.DataSets;
public partial class Internet : DataSet
{
    /// <summary>
    /// The source to pull names from.
    /// </summary>
    internal protected Name Name;

    public Internet()
    {
        Name = Notifier.Flow(new Name());
    }
    /// <summary>
    /// Generates a legit Internet URL avatar from twitter accounts.
    /// </summary>
    /// <returns>A string containing a URL avatar from twitter accounts.</returns>
    public string Avatar()
    {
        return ee1.Avatar(this);
    }
    /// <summary>
    /// Generates an email address.
    /// </summary>
    /// <param name="firstName">Always use this first name.</param>
    /// <param name="lastName">Sometimes used depending on randomness. See 'UserName'.</param>
    /// <param name="provider">Always use the provider.</param>
    /// <param name="uniqueSuffix">This parameter is appended to
    /// the email account just before the @ symbol. This is useful for situations
    /// where you might have a unique email constraint in your database or application.
    /// Passing var f = new Faker(); f.UniqueIndex is a good choice. Or you can supply
    /// your own unique changing suffix too like Guid.NewGuid; just be sure to change the
    /// <paramref name="uniqueSuffix"/> value each time before calling this method
    /// to ensure that email accounts that are generated are totally unique.</param>
    /// <returns>An email address</returns>
    public string Email(string? firstName = null, string? lastName = null, string? provider = null, string? uniqueSuffix = null)
    {
        return ee1.Email(this, firstName, lastName, provider, uniqueSuffix);
    }
    /// <summary>
    /// Generates an example email with @example.com.
    /// </summary>
    /// <param name="firstName">Optional: first name of the user.</param>
    /// <param name="lastName">Optional: last name of the user.</param>
    /// <returns>An example email ending with @example.com.</returns>
    public string ExampleEmail(string? firstName = null, string? lastName = null)
    {
        return ee1.ExampleEmail(this, firstName, lastName);
    }
    /// <summary>
    /// Generates user names.
    /// </summary>
    /// <param name="firstName">First name is always part of the returned user name.</param>
    /// <param name="lastName">Last name may or may not be used.</param>
    /// <returns>A random user name.</returns>
    public string UserName(string? firstName = null, string? lastName = null)
    {
        return ee1.UserName(this, firstName, lastName);
    }
    /// <summary>
    /// Generates a user name preserving Unicode characters.
    /// </summary>
    /// <param name="firstName">First name is always part of the returned user name.</param>
    /// <param name="lastName">Last name may or may not be used.</param>
    public string UserNameUnicode(string? firstName = null, string? lastName = null)
    {
        return ee1.UserNameUnicode(this, firstName, lastName);
    }
    /// <summary>
    /// Generates a random domain name.
    /// </summary>
    /// <returns>A random domain name.</returns>
    public string DomainName()
    {
        return ee1.DomainName(this);
    }

    /// <summary>
    /// Generates a domain word used for domain names.
    /// </summary>
    /// <returns>A random domain word.</returns>
    public string DomainWord()
    {
        return ee1.DomainWord(this);
    }
    /// <summary>
    /// Generates a domain name suffix like .com, .net, .org
    /// </summary>
    /// <returns>A random domain suffix.</returns>
    public string DomainSuffix()
    {
        return ee1.DomainSuffix(this);
    }
    [GeneratedRegex(@"([\\ ~#&*{}/:<>?|\""'])")]
    internal static partial Regex MyRegex1();

    /// <summary>
    /// Gets a random IPv4 address string.
    /// </summary>
    /// <returns>A random IPv4 address.</returns>
    public string Ip()
    {
        return $"{Random.Number(1, 255)}.{Random.Number(255)}.{Random.Number(255)}.{Random.Number(255)}";
    }

    /// <summary>
    /// Generates a random port number.
    /// </summary>
    /// <returns>A random port number</returns>
    public int Port()
    {
        return Random.Number(min: IPEndPoint.MinPort + 1, max: IPEndPoint.MaxPort);
    }

    /// <summary>
    /// Gets a random IPv4 IPAddress type.
    /// </summary>
    public IPAddress IpAddress()
    {
        var bytes = Random.Bytes(4);
        if (bytes[0] == 0)
        {
            bytes[0]++;
        }
        var address = new IPAddress(bytes);
        return address;
    }

    /// <summary>
    /// Gets a random IPv4 IPEndPoint.
    /// </summary>
    /// <returns>A random IPv4 IPEndPoint.</returns>
    public IPEndPoint IpEndPoint()
    {
        var address = IpAddress();
        var port = Random.Int(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort);
        return new IPEndPoint(address, port);
    }

    /// <summary>
    /// Generates a random IPv6 address string.
    /// </summary>
    /// <returns>A random IPv6 address.</returns>
    public string Ipv6()
    {
        var bytes = Random.Bytes(16);
        return
           $"{bytes[0]:x}{bytes[1]:x}:{bytes[2]:x}{bytes[3]:x}:{bytes[4]:x}{bytes[5]:x}:{bytes[6]:x}{bytes[7]:x}:{bytes[8]:x}{bytes[9]:x}:{bytes[10]:x}{bytes[11]:x}:{bytes[12]:x}{bytes[13]:x}:{bytes[14]:x}{bytes[15]:x}";
    }

    /// <summary>
    /// Generate a random IPv6 IPAddress type.
    /// </summary>
    /// <returns></returns>
    public IPAddress Ipv6Address()
    {
        var address = new IPAddress(Random.Bytes(16));
        return address;
    }

    /// <summary>
    /// Gets a random IPv6 IPEndPoint.
    /// </summary>
    /// <returns>A random IPv6 IPEndPoint.</returns>
    public IPEndPoint Ipv6EndPoint()
    {
        var address = Ipv6Address();
        var port = Random.Int(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort);
        return new IPEndPoint(address, port);
    }
    /// <summary>
    /// Gets a random mac address.
    /// </summary>
    /// <param name="separator">The string the mac address should be separated with.</param>
    /// <returns>A random mac address.</returns>
    public string Mac(string separator = ":")
    {
        var arr = Enumerable.Range(0, 6)
           .Select(_ => Random.Number(0, 255).ToString("x2"));

        return string.Join(separator, arr);
    }
    /// <summary>
    /// Generates a random password.
    /// </summary>
    /// <param name="password">Class with all settings needed for password.  Set to null to specify will use default one.</param>
    /// <returns>A random password.</returns>
    public string Password(RandomPasswordParameterClass? password = null)
    {
        return ee1.Password(this, password);
    }
    /// <summary>
    /// Gets a random aesthetically pleasing color near the base RGB. See [here](http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette).
    /// </summary>
    /// <param name="baseRed">Red base color</param>
    /// <param name="baseGreen">Green base color</param>
    /// <param name="baseBlue">Blue base color</param>
    /// <param name="grayscale">Output a gray scale color</param>
    /// <param name="format">The color format</param>
    /// <returns>A random color.</returns>
    public string Color(byte baseRed = 0, byte baseGreen = 0, byte baseBlue = 0, bool grayscale = false, EnumColorFormat format = EnumColorFormat.Hex)
    {
        var red = Math.Floor((Random.Number(256) + (double)baseRed) / 2);
        var green = Math.Floor((Random.Number(256) + (double)baseGreen) / 2);
        var blue = Math.Floor((Random.Number(256) + (double)baseBlue) / 2);
        if (grayscale)
        {
            green = red;
            blue = red;
        }
        var r = (byte)red;
        var g = (byte)green;
        var b = (byte)blue;
        if (format == EnumColorFormat.Hex)
        {
            return $"#{r:x02}{g:x02}{b:x02}";
        }
        if (format == EnumColorFormat.Delimited)
        {
            return DelimitedRgb();
        }
        return $"rgb({DelimitedRgb()})";
        string DelimitedRgb()
        {
            return $"{r},{g},{b}";
        }
    }
    /// <summary>
    /// Returns a random protocol. HTTP or HTTPS.
    /// </summary>
    /// <returns>A random protocol.</returns>
    public string Protocol()
    {
        BasicList<string> protocols = [ "http", "https" ];
        return Random.ListItem(protocols);
    }
    public string Url(string protocol = "https", string? domain = null, string? domainPrefix = null, string? path = null, BasicList<string>? extensions = null)
    {
        return ee1.Url(this, protocol, domain, domainPrefix, path, extensions);
    }
    public string TwitterName()
    {
        return ee1.TwitterName(this);
    }
    public string HashTag()
    {
        return ee1.HashTag(this);
    }
    public string GeoHash()
    {
        return ee1.GeoHash(this);
    }
}