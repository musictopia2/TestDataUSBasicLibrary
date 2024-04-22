namespace TestDataUSBasicLibrary;

/// <summary>
/// Represents a Faker rule
/// </summary>
public class Rule<T>
{
    /// <summary>
    /// Populate action
    /// </summary>
    public T? Action { get; set; }

    /// <summary>
    /// Property name, maybe null for finalize or create.
    /// </summary>
    public string PropertyName { get; set; } = ""; //iffy.

    /// <summary>
    /// The rule set this rule belongs to.
    /// </summary>
    public string RuleSet { get; set; } = string.Empty;

    /// <summary>
    /// Prohibits the rule from being applied in strict mode.
    /// </summary>
    public bool ProhibitInStrictMode { get; set; } = false;
}

public class PopulateAction<T> : Rule<Func<Faker, T, object>>
{
}

public class FinalizeAction<T> : Rule<Action<Faker, T>>
{
}

public class MultiDictionary<Key, Key2, Value>(IEqualityComparer<Key> comparer) : Dictionary<Key, Dictionary<Key2, Value>>(comparer)
    where Key : notnull
    where Key2: notnull
{
    public void Add(Key key, Key2 key2, Value value)
    {
        if (!TryGetValue(key, out var values))
        {
            values = [];
            Add(key, values);
        }
        values[key2] = value;
    }
}
public class MultiSetDictionary<Key, Value>(IEqualityComparer<Key> comparer) : Dictionary<Key, HashSet<Value>>(comparer)
    where Key : notnull
{
    public void Add(Key key, Value value)
    {
        if (!TryGetValue(key, out var values))
        {
            values = [];
            Add(key, values);
        }
        if (values.Contains(value))
        {
            throw new ArgumentException("An item with the same key has already been added.");
        }
        values.Add(value);
    }
}