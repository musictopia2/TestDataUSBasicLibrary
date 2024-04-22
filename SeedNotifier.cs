namespace TestDataUSBasicLibrary;
/// <summary>
/// The seed notifier's purpose is to keep track of any objects that
/// might need to be notified when a seed/randomizer changes.
/// For example, the Internet dataset depends on the Name dataset 
/// to generate data. If the randomizer seed changes in Internet, the 
/// Name dependency data set should be notified of this change too.
/// This whole process is important in maintaining determinism.
/// </summary>
public class SeedNotifier
{
    private readonly BasicList<IHasRandomizer> _registry = [];
    /// <summary>
    /// Causes <paramref name="item"/> to be remembered and tracked so that the
    /// <paramref name="item"/> will be notified when <see cref="Notify"/> is called.
    /// </summary>
    public U Flow<U>(U item) where U : IHasRandomizer
    {
        _registry.Add(item);
        return item;
    }

    /// <summary>
    /// Pushes/notifies all tracked objects that a new randomizer has been set.
    /// </summary>
    public void Notify(Randomizer r)
    {
        foreach (var item in _registry)
        {
            item.Random = r;
        }
    }

}