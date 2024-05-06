using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.DataSetFunctions<TestDataUSBasicLibrary.DataSet>;
namespace TestDataUSBasicLibrary;
/// <summary>
/// Data set methods that access specialized data
/// </summary>
public class DataSet : IHasRandomizer, IMustashable
{
    protected internal static IRandomData GetCustomData => rr3.GetRandomDataClass();
    SeedNotifier IHasRandomizer.GetNotifier()
    {
        return Notifier;
    }
    //so if the dataset wants to use a filtered list, can do.
    public IRandomizer RandomFiltered => _randomizer!;
    /// <summary>
    /// See <see cref="SeedNotifier"/>.
    /// </summary>
    protected SeedNotifier Notifier = new();
    private Randomizer? _randomizer;
    /// <summary>
    /// Gets or sets the <see cref="Randomizer"/> used to generate values.
    /// </summary>
    public Randomizer Random
    {
        get => _randomizer ?? (Random = new Randomizer());
        set
        {
            _randomizer = value;
            Notifier.Notify(value);
        }
    }
    /// <summary>
    /// Get a random color.
    /// </summary>
    /// <returns>A random color.</returns>
    public string Color()
    {
        return ee1.Color(this);
    }

}