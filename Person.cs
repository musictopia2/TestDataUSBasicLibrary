namespace TestDataUSBasicLibrary;
/// <summary>
/// Uses Faker to generate a person with contextually relevant fields.
/// </summary>
public class Person : IHasRandomizer, IHasContext
{
    //context variable to store state from Bogus.Extensions so, they
    //keep returning the result on each person.
    internal Dictionary<string, object> _context = [];
    Dictionary<string, object> IHasContext.Context => _context;
    public class CardAddress
    {
        public class CardGeo
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public string Street { get; set; } = "";
        public string Suite { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public CardGeo? Geo { get; set; }
    }

    protected internal Name? DsName { get; set; }
    protected internal Internet? DsInternet { get; set; }
    protected internal Date? DsDate { get; set; }
    protected internal PhoneNumbers? DsPhoneNumbers { get; set; }
    protected internal Address? DsAddress { get; set; }
    /// <summary>
    /// Creates a new Person object.
    /// </summary>
    /// <param name="locale">The locale to use. Defaults to 'en'.</param>
    /// <param name="seed">The seed used to generate person data. When a <paramref name="seed"/> is specified,
    /// the Randomizer.Seed global static is ignored and a locally isolated derived seed is used to derive randomness.
    /// However, if the <paramref name="seed"/> parameter is null, then the Randomizer.Seed global static is used to derive randomness.
    /// </param>
    public Person(int? seed = null, DateTime? refDate = null)
    {
        GetDataSources();
        if (seed.HasValue)
        {
            Random = new Randomizer(seed.Value);
        }
        if (refDate.HasValue)
        {
            DsDate!.LocalSystemClock = () => refDate.Value;
        }
        Populate();
    }
    internal Person(Randomizer randomizer, DateTime? refDate)
    {
        GetDataSources();
        Random = randomizer;
        if (refDate.HasValue)
        {
            DsDate!.LocalSystemClock = () => refDate.Value;
        }
        Populate();
    }
    private void GetDataSources()
    {
        DsName = Notifier.Flow(new Name());
        DsInternet = Notifier.Flow(new Internet());
        DsDate = Notifier.Flow(new Date ());
        DsPhoneNumbers = Notifier.Flow(new PhoneNumbers());
        DsAddress = Notifier.Flow(new Address());
    }
    protected internal virtual void Populate()
    {
        Gender = DsName!.Gender();
        FirstName = DsName.FirstName(Gender);
        LastName = DsName.LastName();
        FullName = $"{FirstName} {LastName}";
        UserName = DsInternet!.UserName(FirstName, LastName);
        Email = DsInternet.Email(FirstName, LastName);
        Website = DsInternet.DomainName();
        Avatar = DsInternet.Avatar();
        DateOfBirth = DsDate!.Past(50, DsDate.GetTimeReference().AddYears(-20));
        Phone = DsPhoneNumbers!.PhoneNumber();
        Address = new CardAddress
        {
            Street = DsAddress!.FullAddress(),
            Suite = DsAddress.SecondaryAddress(),
            City = DsAddress.City(),
            State = DsAddress.State(),
            ZipCode = DsAddress.ZipCode(),
            Geo = new CardAddress.CardGeo
            {
                Lat = DsAddress.Latitude(),
                Lng = DsAddress.Longitude()
            }
        };
    }
    protected SeedNotifier Notifier = new ();
    private Randomizer? _randomizer;
    public Randomizer Random
    {
        get => _randomizer ?? (Random = new Randomizer());
        set
        {
            _randomizer = value;
            Notifier.Notify(value);
        }
    }
    SeedNotifier IHasRandomizer.GetNotifier()
    {
        return Notifier;
    }
    public EnumGender Gender { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Avatar { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DateOfBirth;
    public CardAddress? Address { get; set; }
    public string Phone { get; set; } = "";
    public string Website { get; set; } = "";
}