namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class InternetFunctions<T>
    where T: Internet
{
    public static Func<T, string> Avatar { get; set; } = internet =>
    {
        var n = internet.Random.Number(0, 1249);
        return $"https://cloudflare-ipfs.com/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/{n}.jpg";
    };
    public static Func<T, string?, string?, string?, string?, string> Email { get; set; } =
        (obj, firstName, lastName, provider, uniqueSuffix) =>
        {
            BasicList<string> freeEmails =
            [
            "gmail.com",
            "yahoo.com",
            "hotmail.com"
            ];
            provider ??= obj.Random.ListItem(freeEmails);

            return obj.UserName(firstName, lastName) + uniqueSuffix + "@" + provider;
        };
    public static Func<T, string?, string?, string> ExampleEmail { get; set; } =
        (obj, firstName, lastName) =>
        {
            BasicList<string> exampleEmails =
            [
            "example.org",
            "example.com",
            "example.net"
            ];
            var provider = obj.Random.ListItem(exampleEmails);
            return obj.Email(firstName, lastName, provider);
        };
    public static Func<T, string?, string?, string> UserName { get; set; } =
        (obj, firstName, lastName) =>
        {
            firstName ??= obj.Name.FirstName();
            lastName ??= obj.Name.LastName();
            return Utils.Slugify(obj.UserNameUnicode(firstName, lastName));
        };
    public static Func<T, string?, string?, string> UserNameUnicode { get; set; } =
        (obj, firstName, lastName) =>
        {
            firstName ??= obj.Name.FirstName();
            lastName ??= obj.Name.LastName();
            var val = obj.Random.Number(2);
            string result;
            BasicList<string> list =
                [
                ".",
            "_"
                ];

            if (val == 0)
            {
                result = firstName + obj.Random.Number(99);
            }
            else if (val == 1)
            {
                result = firstName + obj.Random.ListItem(list) + lastName;
            }
            else
            {
                result = firstName + obj.Random.ListItem(list) + lastName + obj.Random.Number(99);
            }

            result = result.Replace(" ", string.Empty);
            return result;
        };
    public static Func<T, string> DomainName { get; set; } = obj =>
    {
        return obj.DomainWord() + "." + obj.DomainSuffix();
    };
    public static Func<T, string> DomainWord { get; set; } = obj =>
    {
        var domain = obj.Name.FirstName().ToLower();
        return Internet.MyRegex1().Replace(domain, string.Empty);
    };
    public static Func<T, string> DomainSuffix { get; set; } = obj =>
    {
        BasicList<string> domains =
            [
              "com",
              "biz",
              "info",
              "name",
              "net",
              "org"
            ];
        return obj.Random.ListItem(domains);
    };
    public static Func<T, RandomPasswordParameterClass?, string> Password { get; set; } = (obj, password) =>
    {
        IRandomNumberList random = obj.Random;
        if (password is not null)
        {
            return random.Password(password);
        }
        return random.Password();
    };
    public static Func<T, string, string?, string?, string?, BasicList<string>?, string> Url { get; set; } =
        (obj, protocol, domain, domainPrefix, path, extensions) =>
        {
            IRandomNumberList random = obj.Random;
            return random.NextUrl(protocol, domain, domainPrefix, path, extensions);
        };
    public static Func<T, string> TwitterName { get; set; } = obj =>
    {
        IRandomNumberList random = obj.Random;
        return random.NextTwitterName();
    };
    public static Func<T, string> HashTag { get; set; } = obj =>
    {
        IRandomNumberList random = obj.Random;
        return random.NextHashtag();
    };
    public static Func<T, string> GeoHash { get; set; } = obj =>
    {
        IRandomNumberList random = obj.Random;
        return random.NextGeohash();
    };

}