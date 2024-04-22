namespace TestDataUSBasicLibrary;
public class Faker : IHasRandomizer, IHasContext
{
    /// <summary>
    /// The default mode to use when generating objects. Strict mode ensures that all properties have rules.
    /// </summary>
    public static bool DefaultStrictMode { get; set; } = false;
    //eventually can use this (?)
    Dictionary<string, object> IHasContext.Context { get; } = [];

    /// <summary>
    /// See <see cref="SeedNotifier"/>
    /// </summary>
    protected SeedNotifier Notifier = new();
    SeedNotifier IHasRandomizer.GetNotifier()
    {
        return Notifier;
    }
    private Randomizer? _randomizer;

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate numbers, booleans, and decimals.
    /// </summary>
    public IRandomizer Random
    {
        get => _randomizer ?? (Random = new Randomizer());
        set
        {
            if (value is Randomizer rans)
            {
                _randomizer = rans;
                Notifier.Notify(rans);
                return;
            }
            throw new CustomBasicException("Only a standard randomizer is supported.  Did this way so intellisense would work the way as intended in visual studio when using fakers");
        }
    }
    Randomizer IHasRandomizer.Random { set => _randomizer = value; }
    public Faker()
    {
        Address = Notifier.Flow(new Address());
        Date = Notifier.Flow(new Date());
        Finance = Notifier.Flow(new Finance());
        Image = Notifier.Flow(new Images());
        Internet = Notifier.Flow(new Internet());
        Lorem = Notifier.Flow(new Lorem());
        Name = Notifier.Flow(new Name());
        Phone = Notifier.Flow(new PhoneNumbers());
        Commerce = Notifier.Flow(new Commerce());
        Vehicle = Notifier.Flow(new Vehicle());
        Music = Notifier.Flow(new Music());
        Events = Notifier.Flow(new Events());
        Hashids = new Hashids();
    }
    private DateTime? _localDateTimeRef;

    /// <summary>
    /// The fixed point in time DateTime reference used for date and time calculations
    /// with this Faker instance and the underlying .Date dataset. If this property is set to null,
    /// then the .Date dataset's static system clock is usually used.
    /// 
    /// Typically, this property is set when Faker[T].UseDateTimeReference() is called,
    /// or is set manually when creating an instance of new Faker { DateTimeReference = new DateTime(year, month, day) }.
    /// When this property is set, all date/time calculations from .Date will begin calculations from this fixed point in time for this Faker instance.
    /// </summary>
    public DateTime? DateTimeReference
    {
        get
        {
            return _localDateTimeRef;
        }
        set
        {
            _localDateTimeRef = value;
            if (_localDateTimeRef.HasValue)
            {
                this.Date.LocalSystemClock = () => _localDateTimeRef.Value;
            }
            else
            {
                this.Date.LocalSystemClock = null;
            }
        }
    }

    


    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Phone Numbers
    /// </summary>
    public PhoneNumbers Phone { get; set; }


    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Names
    /// </summary>
    public Name Name { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Words
    /// </summary>
    public Lorem Lorem { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Image URL Links
    /// </summary>
    public Images Image { get; set; }

    //[RegisterMustasheMethods]
    /// <summary>
    /// Generate Finance Items
    /// </summary>
    public Finance Finance { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Addresses
    /// </summary>
    public Address Address { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Dates
    /// </summary>
    public Date Date { get; set; }


    //[RegisterMustasheMethods]

    /// <summary>
    /// Generate Internet stuff like Emails and UserNames.
    /// </summary>
    public Internet Internet { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generates data related to commerce
    /// </summary>
    public Commerce Commerce { get; set; }


    //[RegisterMustasheMethods]

    /// <summary>
    /// Generates data related to vehicles.
    /// </summary>
    public Vehicle Vehicle { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generates data related to music.
    /// </summary>
    public Music Music { get; set; }

    //[RegisterMustasheMethods]

    /// <summary>
    /// Generates data related to history events.
    /// </summary>
    public Events Events { get; set; }

    /// <summary>
    /// Helper method to pick a random element.
    /// </summary>
    public T PickRandom<T>(BasicList<T> items)
    {
        return Random.ListItem(items);
    }

    private void AvoidStatic()
    {
        if (Random is null)
        {
            return;
        }
    }

    /// <summary>
    /// Helper to pick random subset of elements out of the list.
    /// </summary>
    /// <param name="amountToPick">amount of elements to pick of the list.</param>
    /// <exception cref="ArgumentException">if amountToPick is lower than zero.</exception>
    public IBasicList<T> PickRandom<T>(BasicList<T> items, int amountToPick)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amountToPick);
        var size = items.Count;
        ArgumentOutOfRangeException.ThrowIfGreaterThan(amountToPick, size);
        return Random.ListItems(items, amountToPick);
    }
    /// <summary>
    /// Helper method to call faker actions multiple times and return the result as BasicList of T
    /// </summary>
    public BasicList<T> Make<T>(int count, Func<T> action)
    {
        AvoidStatic();
        BasicList<T> output = [];
        count.Times(() =>
        {
            output.Add(action.Invoke());
        });
        return output;
    }
    /// <summary>
    /// Helper method to call faker actions multiple times and return the result as BasicList of T.
    /// This method passes in the current index of the generation.
    /// </summary>
    public BasicList<T> Make<T>(int count, Func<int, T> action)
    {
        AvoidStatic();
        BasicList<T> output = [];
        count.Times(x =>
        {
            output.Add(action.Invoke(x));
        });
        return output;
    }

    /// <summary>
    /// Returns an IEnumerable[T] with LINQ deferred execution. Generated values
    /// are not guaranteed to be repeatable until .ToBasicList() is called.
    /// </summary>
    public IEnumerable<T> MakeLazy<T>(int count, Func<T> action)
    {
        AvoidStatic();
        return Enumerable.Range(1, count).Select(n => action());
    }

    /// <summary>
    /// Same as Make() except this method passes in the current index of the generation. Also,
    /// returns an IEnumerable[T] with LINQ deferred execution. Generated values are not
    /// guaranteed to be repeatable until .ToBasicList() is called.
    /// </summary>
    public IEnumerable<T> MakeLazy<T>(int count, Func<int, T> action)
    {
        AvoidStatic();
        return Enumerable.Range(1, count).Select(action);
    }



    /// <summary>
    /// Triggers a new generation context
    /// </summary>
    internal void NewContext()
    {
        //person = null;
        _capturedGlobalIndex = Interlocked.Increment(ref GlobalUniqueIndex);
        Interlocked.Increment(ref IndexFaker);
    }

    /// <summary>
    /// Checks if the internal state is ready to be used by <seealso cref="Faker{T}"/>.
    /// In other words, has NewContext ever been called since this object was created?
    /// See Issue 143. https://github.com/bchavez/Bogus/issues/143
    /// </summary>
    internal bool HasContext => IndexFaker != -1;

    /// <summary>
    /// A global variable that is automatically incremented on every
    /// new object created by Bogus. Useful for composing property values that require
    /// uniqueness.
    /// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible  this has to be exception.
    public static int GlobalUniqueIndex = -1;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    private int _capturedGlobalIndex;

    /// <summary>
    /// Alias for IndexGlobal.
    /// </summary>
    //[Obsolete("Please use IndexGlobal instead.")]
    public int UniqueIndex => _capturedGlobalIndex;

    /// <summary>
    /// A global static variable that is automatically incremented on every
    /// new object created by Bogus across all Faker[T]s in the entire application.
    /// Useful for composing property values that require uniqueness across
    /// the entire application.
    /// </summary>
    public int IndexGlobal => _capturedGlobalIndex;

    /// <summary>
    /// A local variable that is automatically incremented on every
    /// new object generated by the Faker[T] instance for lifetime of Faker[T].
    /// </summary>
    public int IndexFaker = -1;

    /// <summary>
    /// A local index variable that can be controlled inside rules with ++ and --.
    /// This variable's lifetime exists for the lifetime of Faker[T].
    /// </summary>
    public int IndexVariable = 0;

    /// <summary>
    /// HashID generator with default (string.Empty) salt.See: https://github.com/ullmark/hashids.net
    /// </summary>
    public Hashids Hashids { get; set; }
}