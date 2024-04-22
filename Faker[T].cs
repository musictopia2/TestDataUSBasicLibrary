namespace TestDataUSBasicLibrary;
public class Faker<T> : IRuleSet<T>
    where T : class
{
    private readonly Dictionary<string, PropertyMapper<T>> _mappings = [];
    protected const string Default = "default";
    private static readonly string[] _defaultRuleSet = [Default];
    protected internal Faker FakerHub;
    protected internal readonly MultiDictionary<string, string, PopulateAction<T>> Actions =
      new(StringComparer.OrdinalIgnoreCase);
    protected internal readonly Dictionary<string, FinalizeAction<T>> FinalizeActions = new(StringComparer.OrdinalIgnoreCase);
    protected internal Dictionary<string, Func<Faker, T>> CreateActions = new(StringComparer.OrdinalIgnoreCase);
    protected internal Dictionary<string, bool> StrictModes = [];
    protected internal bool? IsValid { get; set; }
    protected internal string _currentRuleSet = Default;
    protected internal int? _localSeed; // if null, the global Randomizer.Seed is used.
    protected internal DateTime? _localDateTimeRef;
    public Faker()
    {
        if (TestGeneratorHelpersGlobal<T>.MasterContext is null)
        {
            throw new CustomBasicException("There was nothing registered for mapping properties.  Try creating a source generator and registering it");
        }
        _mappings = TestGeneratorHelpersGlobal<T>.MasterContext.GetProperties();
        CreateActions[Default] = faker => TestGeneratorHelpersGlobal<T>.MasterContext.CreateNewObject();
        FakerHub = StartFaker(); //so if you need another faker for other things, you have that choice.
    }
    protected virtual Faker StartFaker()
    {
        return new();
    }

    /// <summary>
    /// Clones the internal state of a <seealso cref="Faker{T}"/> into a new <seealso cref="Faker{T}"/> so that
    /// both are isolated from each other. The clone will have internal state
    /// reset as if <seealso cref="Generate(string)"/> was never called.
    /// </summary>
    public Faker<T> Clone()
    {
        var clone = new Faker<T>();

        //copy internal state.
        //strict modes.
        foreach (var root in StrictModes)
        {
            clone.StrictModes.Add(root.Key, root.Value);
        }

        //create actions
        foreach (var root in CreateActions)
        {
            clone.CreateActions[root.Key] = root.Value;
        }
        //finalize actions
        foreach (var root in FinalizeActions)
        {
            clone.FinalizeActions.Add(root.Key, root.Value);
        }

        //actions
        foreach (var root in Actions)
        {
            foreach (var kv in root.Value)
            {
                clone.Actions.Add(root.Key, kv.Key, kv.Value);
            }
        }
        if (_localSeed.HasValue)
        {
            clone.UseSeed(_localSeed.Value);
        }
        if (_localDateTimeRef.HasValue)
        {
            clone.UseDateTimeReference(_localDateTimeRef.Value);
        }
        return clone;
    }
    /// <summary>
    /// Creates a seed locally scoped within this <seealso cref="Faker{T}"/> ignoring the globally scoped <seealso cref="Randomizer.Seed"/>.
    /// If this method is never called the global <seealso cref="Randomizer.Seed"/> is used.
    /// </summary>
    /// <param name="seed">The seed value to use within this <seealso cref="Faker{T}"/> instance.</param>
    public virtual Faker<T> UseSeed(int seed)
    {
        _localSeed = seed;
        FakerHub.Random = new Randomizer(seed);
        return this;
    }
    /// <summary>
    /// Sets a local time reference for all DateTime calculations used by
    /// this Faker[T] instance; unless refDate parameters are specified 
    /// with the corresponding Date.Methods().
    /// </summary>
    /// <param name="refDate">The anchored DateTime reference to use.</param>
    public virtual Faker<T> UseDateTimeReference(DateTime? refDate)
    {
        _localDateTimeRef = refDate;
        FakerHub.DateTimeReference = refDate;
        return this;
    }

    /// <summary>
    /// Instructs <seealso cref="Faker{T}"/> to use the factory method as a source
    /// for new instances of <typeparamref name="T"/>.
    /// </summary>
    public virtual Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod)
    {
        CreateActions[_currentRuleSet] = factoryMethod;
        return this;
    }
    /// <summary>
    /// Creates a rule for a compound property and providing access to the instance being generated.
    /// </summary>
    public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter)
    {
        var propName = PropertyName.For(property);
        return AddRule(propName, (f, t) => setter(f, t)!);
    }

    /// <summary>
    /// Creates a rule for a property.
    /// </summary>
    public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
    {
        var propName = PropertyName.For(property);
        return AddRule(propName, (f, t) => value!);
    }

    /// <summary>
    /// Creates a rule for a property.
    /// </summary>
    public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<TProperty> valueFunction)
    {
        var propName = PropertyName.For(property);
        return AddRule(propName, (f, t) => valueFunction()!);
    }

    /// <summary>
    /// Creates a rule for a property.
    /// </summary>
    public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter)
    {
        var propName = PropertyName.For(property);
        return AddRule(propName, (f, t) => setter(f)!);
    }
    protected virtual Faker<T> AddRule(string propertyOrField, Func<Faker, T, object> invoker)
    {
        var rule = new PopulateAction<T>
        {
            Action = invoker,
            RuleSet = _currentRuleSet,
            PropertyName = propertyOrField,
        };
        Actions.Add(_currentRuleSet, propertyOrField, rule);
        return this;
    }

    /// <summary>
    /// Specify multiple rules inside an action without having to call
    /// RuleFor multiple times. Note: <seealso cref="StrictMode"/> must be false
    /// since rules for properties and fields cannot be individually checked when
    /// using this method.
    /// </summary>
    public virtual Faker<T> Rules(Action<Faker, T> setActions)
    {
        object invoker(Faker f, T t)
        {
            setActions(f, t);
            return null!;
        }
        var guid = Guid.NewGuid().ToString();
        var rule = new PopulateAction<T>
        {
            Action = invoker,
            RuleSet = _currentRuleSet,
            PropertyName = guid,
            ProhibitInStrictMode = true
        };
        Actions.Add(_currentRuleSet, guid, rule);
        return this;
    }
    /// <summary>
    /// Defines a set of rules under a specific name. Useful for defining
    /// rules for special cases. Note: The name `default` is the name of all rules that are
    /// defined without an explicit rule set.
    /// </summary>
    /// <param name="ruleSetName">The rule set name.</param>
    /// <param name="action">The set of rules to apply when this rules set is specified.</param>
    public virtual Faker<T> RuleSet(string ruleSetName, Action<IRuleSet<T>> action)
    {
        if (_currentRuleSet != Default)
        {
            throw new ArgumentException("Cannot create a rule set within a rule set.");
        }

        _currentRuleSet = ruleSetName;
        action(this);
        _currentRuleSet = Default;
        return this;
    }

    /// <summary>
    /// Creates one rule for all types of <typeparamref name="TType"/> on type <typeparamref name="T"/>.
    /// In other words, if you have <typeparamref name="T"/> with many fields or properties of
    /// type <seealso cref="Int32"/> this method allows you to specify a rule for all fields or
    /// properties of type <seealso cref="Int32"/>.
    /// </summary>
    public virtual Faker<T> RuleForType<TType>(Type type, Func<Faker, TType> setterForType)
    {
        if (typeof(TType) != type)
        {
            throw new ArgumentException($"{nameof(TType)} must be the same type as parameter named '{nameof(type)}'");
        }

        foreach (var kvp in _mappings)
        {
            if (kvp.Value.Type == type && kvp.Value.RuleCategory != EnumRuleCategory.Forbid)
            {
                RuleFor(kvp.Key, setterForType);
            }
        }


        //foreach (var kvp in this.TypeProperties)
        //{
        //    var propOrFieldType = GetFieldOrPropertyType(kvp.Value);
        //    var propOrFieldName = kvp.Key;

        //    if (propOrFieldType == type)
        //    {
        //        RuleFor(propOrFieldName, setterForType);
        //    }
        //}

        return this;
    }

    /// <summary>
    /// Create a rule for a hidden property or field.
    /// Used in advanced scenarios to create rules for hidden properties or fields.
    /// </summary>
    /// <param name="prop">The property name or field name of the member to create a rule for.</param>
    public virtual Faker<T> RuleFor<TProperty>(string prop, Func<Faker, TProperty> setter)
    {
        EnsureMemberExists(prop,
           $"The property or field {prop} was not found on {typeof(T)}. " +
           $"Can't create a rule for {typeof(T)}.{prop} when {prop} " +
           $"cannot be found. Try creating changing the implementation of IMapPropertiesForTesting or fix those bugs  " +
           $"in order to capture the value for {typeof(T)}.");

        return AddRule(prop, (f, t) => setter(f)!);
    }

    /// <summary>
    /// Create a rule for a hidden property or field.
    /// Used in advanced scenarios to create rules for hidden properties or fields.
    /// </summary>
    /// <param name="prop">The property name or field name of the member to create a rule for.</param>
    public virtual Faker<T> RuleFor<TProperty>(string prop, Func<Faker, T, TProperty> setter)
    {
        EnsureMemberExists(prop,
           $"The property or field {prop} was not found on {typeof(T)}. " +
           $"Can't create a rule for {typeof(T)}.{prop} when {prop} " +
           $"cannot be found. Try creating changing the implementation of IMapPropertiesForTesting or fix those bugs  " +
           $"in order to capture the value for {typeof(T)}.");

        return AddRule(prop, (f, t) => setter(f, t)!);
    }


    /// <summary>
    /// Ignores a property or field when <seealso cref="StrictMode"/> is enabled.
    /// Used in advanced scenarios to ignore hidden properties or fields.
    /// </summary>
    /// <param name="propertyName">The property name of the member to ignore.</param>
    public virtual Faker<T> Ignore(string propertyName)
    {
        EnsureMemberExists(propertyName,
           $"The property {propertyName} was not found on {typeof(T)}. " +
           $"Can't ignore member {typeof(T)}.{propertyName} when {propertyName} " +
           $"cannot be found. Try creating changing the implementation of IMapPropertiesForTesting or fix those bugs  " +
           $"in order to capture the value for {typeof(T)}.");
        var rule = new PopulateAction<T>
        {
            Action = null,
            RuleSet = _currentRuleSet,
            PropertyName = propertyName
        };

        Actions.Add(_currentRuleSet, propertyName, rule);

        return this;
    }

    /// <summary>
    /// Ignores a property when <seealso cref="StrictMode"/> is enabled.
    /// </summary>
    public virtual Faker<T> Ignore<TPropertyOrField>(Expression<Func<T, TPropertyOrField>> property)
    {
        var propNameOrField = PropertyName.For(property);

        return Ignore(propNameOrField);
    }

    /// <summary>
    /// When set to true, ensures all properties and public fields of <typeparamref name="T"/> have rules
    /// before an object of <typeparamref name="T"/> is populated or generated. Manual assertion
    /// can be invoked using <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>.
    /// </summary>
    /// <param name="ensureRulesForAllProperties">Overrides any global setting in <seealso cref="Faker.DefaultStrictMode"/>.</param>
    public virtual Faker<T> StrictMode(bool ensureRulesForAllProperties)
    {
        StrictModes[_currentRuleSet] = ensureRulesForAllProperties;
        return this;
    }

    /// <summary>
    /// A finalizing action rule applied to <typeparamref name="T"/> after all the rules
    /// are executed.
    /// </summary>
    public virtual Faker<T> FinishWith(Action<Faker, T> action)
    {
        var rule = new FinalizeAction<T>
        {
            Action = action,
            RuleSet = _currentRuleSet
        };
        FinalizeActions[_currentRuleSet] = rule;
        return this;
    }

    //i propose this is for properties, not fields at all.

    /// <summary>
    /// Ensures a member exists provided by the implementations of the mappers.
    /// </summary>
    protected virtual void EnsureMemberExists(string propName, string exceptionMessage)
    {
        if (_mappings.ContainsKey(propName) == false)
        {
            throw new ArgumentException(exceptionMessage);
        }
    }
    /// <summary>
    /// Utility method to parse out rule sets form user input.
    /// </summary>
    protected virtual BasicList<string> ParseDirtyRulesSets(string dirtyRules)
    {
        dirtyRules = dirtyRules?.Trim(',').Trim()!;
        if (string.IsNullOrWhiteSpace(dirtyRules))
        {
            return _defaultRuleSet.ToBasicList();
        }
        return dirtyRules.Split(',')
           .Where(s => !string.IsNullOrWhiteSpace(s))
           .Select(s => s.Trim()).ToBasicList();
    }
    /// <summary>
    /// Generates a fake object of <typeparamref name="T"/> using the specified rules in this
    /// <seealso cref="Faker{T}"/>.
    /// </summary>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    public virtual T Generate(string? ruleSets = null)
    {
        var cleanRules = ParseDirtyRulesSets(ruleSets!);

        Func<Faker, T> createRule;
        if (string.IsNullOrWhiteSpace(ruleSets))
        {
            createRule = CreateActions[Default];
        }
        else
        {
            var firstRule = cleanRules[0];
            createRule = CreateActions.TryGetValue(firstRule, out createRule!) ? createRule : CreateActions[Default];
        }

        //Issue 143 - We need a new FakerHub context before calling the
        //            constructor. Associated Issue 57: Again, before any
        //            rules execute, we need a context to capture IndexGlobal
        //            and IndexFaker variables.
        FakerHub!.NewContext();
        var instance = createRule(this.FakerHub);

        PopulateInternal(instance, cleanRules);

        return instance;
    }

    /// <summary>
    /// Generates a <seealso cref="List{T}"/> fake objects of type <typeparamref name="T"/> using the specified rules in
    /// this <seealso cref="Faker{T}"/>.
    /// </summary>
    /// <param name="count">The number of items to create in the <seealso cref="List{T}"/>.</param>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    public virtual List<T> Generate(int count, string? ruleSets = null)
    {
        return Enumerable.Range(1, count)
           .Select(i => Generate(ruleSets))
           .ToList();
    }

    /// <summary>
    /// Returns an <seealso cref="IEnumerable{T}"/> with LINQ deferred execution. Generated values
    /// are not guaranteed to be repeatable until <seealso cref="Enumerable.ToList{T}"/> is called.
    /// </summary>
    /// <param name="count">The number of items to create in the <seealso cref="IEnumerable{T}"/>.</param>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    public virtual IEnumerable<T> GenerateLazy(int count, string? ruleSets = null)
    {
        return Enumerable.Range(1, count)
           .Select(i => Generate(ruleSets));
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that can be used as an unlimited source
    /// of <typeparamref name="T"/> when iterated over. Useful for generating unlimited
    /// amounts of data in a memory efficient way. Generated values *should* be repeatable
    /// for a given seed when starting with the first item in the sequence.
    /// </summary>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    public virtual IEnumerable<T> GenerateForever(string? ruleSets = null)
    {
        while (true)
        {
            yield return Generate(ruleSets);
        }
    }

    /// <summary>
    /// Populates an instance of <typeparamref name="T"/> according to the rules
    /// defined in this <seealso cref="Faker{T}"/>.
    /// </summary>
    /// <param name="instance">The instance of <typeparamref name="T"/> to populate.</param>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    public virtual void Populate(T instance, string? ruleSets = null)
    {
        var cleanRules = ParseDirtyRulesSets(ruleSets!);
        PopulateInternal(instance, cleanRules);
    }

    /// <summary>
    /// Populates an instance of <typeparamref name="T"/> according to the rules
    /// defined in this <seealso cref="Faker{T}"/>.
    /// </summary>
    /// <param name="instance">The instance of <typeparamref name="T"/> to populate.</param>
    /// <param name="ruleSets">A comma separated list of rule sets to execute.
    /// Note: The name `default` is the name of all rules defined without an explicit rule set.
    /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
    /// the `default` rules will not run. If you want rules without an explicit rule set to run
    /// you'll need to include the `default` rule set name in the comma separated
    /// list of rules to run. (ex: "ruleSetA, ruleSetB, default")
    /// </param>
    protected virtual void PopulateInternal(T instance, BasicList<string> ruleSets)
    {
        ValidationResult? vr = null;
        if (!IsValid.HasValue)
        {
            //run validation
            vr = ValidateInternal(ruleSets);
            IsValid = vr.IsValid;
        }
        if (!IsValid.GetValueOrDefault())
        {
            throw MakeValidationException(vr ?? ValidateInternal(ruleSets));
        }
        lock (Randomizer._locker.Value)
        {
            //Issue 57 - Make sure you generate a new context
            //           before executing any rules.
            //Issue 143 - If the FakerHub doesn't have any context
            //            (eg NewContext() has never been called), then call it
            //            so we can increment IndexGlobal and IndexFaker.
            if (!FakerHub!.HasContext)
            {
                FakerHub.NewContext();
            }
            foreach (var ruleSet in ruleSets)
            {
                if (Actions.TryGetValue(ruleSet, out var populateActions))
                {
                    foreach (var action in populateActions.Values)
                    {
                        PopulateProperty(instance, action);
                    }
                }
            }
            foreach (var ruleSet in ruleSets)
            {
                if (FinalizeActions.TryGetValue(ruleSet, out FinalizeAction<T>? finalizer))
                {
                    finalizer.Action!(FakerHub, instance);
                }
            }
        }
    }
    private readonly object _setterCreateLock = new();
    private void PopulateProperty(T instance, PopulateAction<T> action)
    {
        var valueFactory = action.Action;
        if (valueFactory is null)
        {
            return; // An .Ignore() rule.
        }
        var value = valueFactory(FakerHub, instance);
        lock (_setterCreateLock)
        {
            bool rets = _mappings.TryGetValue(action.PropertyName, out var details);
            if (rets == false)
            {
                throw new CustomBasicException($"Failed to set property for {action.PropertyName}.  Try running validations for details.  If validations was successful, then bug in system");
            }
            if (details!.Action is null)
            {
                throw new CustomBasicException($"There was no action specified for {action.PropertyName}.  Try running validations for details.  If validatons was successful, the nbug in system");
            }
            details.Action.Invoke(instance, value);
        }
    }
    /// <summary>
    /// When <seealso cref="StrictMode"/> is enabled, checks if all properties or fields of <typeparamref name="T"/> have
    /// rules defined. Returns true if all rules are defined, false otherwise.
    /// The difference between <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>
    /// is that <seealso cref="Validate"/> will *not* throw <seealso cref="ValidationException"/>
    /// if some rules are missing when <seealso cref="StrictMode"/> is enabled.
    /// </summary>
    /// <returns>True if validation passes, false otherwise.</returns>
    public virtual bool Validate(string? ruleSets = null)
    {
        var rules = ruleSets == null
           ? [.. Actions.Keys]
           : ParseDirtyRulesSets(ruleSets);
        var result = ValidateInternal(rules);
        return result.IsValid;
    }

    /// <summary>
    /// Asserts that all properties have rules. When <seealso cref="StrictMode"/> is enabled, an exception will be raised
    /// with complete list of missing rules. Useful in unit tests to catch missing rules at development
    /// time. The difference between <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>
    /// is that <seealso cref="AssertConfigurationIsValid"/> will throw <seealso cref="ValidationException"/>
    /// if some rules are missing when <seealso cref="StrictMode"/> is enabled. <seealso cref="Validate"/>
    /// will not throw an exception and will return <seealso cref="bool"/> true or false accordingly if
    /// rules are missing when <seealso cref="StrictMode"/> is enabled.
    /// </summary>
    /// <exception cref="ValidationException"/>
    public virtual void AssertConfigurationIsValid(string? ruleSets = null)
    {
        BasicList<string> rules;
        if (ruleSets is null)
        {
            rules = [.. Actions.Keys];
        }
        else
        {
            rules = ParseDirtyRulesSets(ruleSets);
        }

        var result = ValidateInternal(rules);
        if (!result.IsValid)
        {
            throw MakeValidationException(result);
        }
    }

    /// <summary>
    /// Composes a <see cref="ValidationException"/> based on the failed validation
    /// results that can be readily used to raise the exception.
    /// </summary>
    protected virtual ValidationException MakeValidationException(ValidationResult result)
    {
        var builder = new StringBuilder();

        result.ExtraMessages.ForEach(m =>
        {
            builder.AppendLine(m);
            builder.AppendLine();
        });

        builder.AppendLine("Validation was called to ensure all properties / fields have rules.")
           .AppendLine($"There are missing rules for Faker<T> '{typeof(T).Name}'.")
           .AppendLine("=========== Missing Rules ===========");

        foreach (var fieldOrProp in result.MissingRules)
        {
            builder.AppendLine(fieldOrProp);
        }

        return new ValidationException(builder.ToString().Trim());
    }
    private Dictionary<string, string> ExtraViolatedRules(string rule)
    {
        //this means you may have other violated rules.
        Dictionary<string, string> output = [];
        foreach (var item in _mappings)
        {
            if (item.Value.RuleCategory == EnumRuleCategory.Force)
            {
                bool rets;
                rets = Actions.TryGetValue(rule, out var populateActions);
                if (rets == false)
                {
                    output.Add(item.Key, $"Did not have populate actions.  However, {item.Key} was forced to have a rule no matter what even though option strict is not on");
                    continue;
                }

                rets = populateActions!.ContainsKey(item.Key);
                if (rets == false)
                {
                    output.Add(item.Key, $"Did not have a rule for {item.Key} even though its forced.  Either find a way to not force it, add the rule or fix the source generators");
                    continue;
                }
            }
        }
        return output;
    }
    private ValidationResult ValidateInternal(BasicList<string> ruleSets)
    {
        var result = new ValidationResult { IsValid = true };
        foreach (var rule in ruleSets)
        {
            if (StrictModes.TryGetValue(rule, out var strictMode))
            {
            }
            else
            {
                strictMode = Faker.DefaultStrictMode;
            }
            Dictionary<string, string> forbidden = [];
            forbidden = ExtraViolatedRules(rule);
            if (forbidden.Count > 0)
            {
                result.IsValid = false;
                result.ExtraMessages.AddRange(forbidden.Values);
                foreach (var item in forbidden.Keys)
                {
                    result.MissingRules.Add(item);
                }
            }
            //If strictMode is not enabled, skip and move on to the next ruleSet.
            if (!strictMode)
            {
                continue;
            }
            Actions.TryGetValue(rule, out var populateActions);
            var userSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (populateActions != null)
            {
                userSet.UnionWith(populateActions.Keys);
            }

            //Get the set properties or fields that are only
            //known to the binder, while removing
            //items in userSet that are known to both the user and binder.

            userSet.SymmetricExceptWith(_mappings.Keys);

            //What's left in userSet is the set of properties or fields
            //that the user does not know about + .Rule() methods.

            if (userSet.Count > 0)
            {
                foreach (var prop in userSet)
                {
                    bool rets;
                    rets = _mappings.TryGetValue(prop, out var used);
                    if (rets == false)
                    {
                        result.ExtraMessages.Add($"There was no property recognized for {prop}.  Try creating a source generator to capture the values");
                        result.IsValid = false;
                        continue;
                    }
                    if (used!.RuleCategory == EnumRuleCategory.Ignore || used.RuleCategory == EnumRuleCategory.Forbid)
                    {
                        //later has to show a rule was forbidden.
                        continue; //because the generator showed this was being ignored or not even allowed.  this means you have more than one way to ignore rules.
                    }
                    if (used.Action is null && used.RuleCategory != EnumRuleCategory.Forbid) //obviously, if you forbid, then no action because i did not want it populated.
                    {
                        result.ExtraMessages.Add($"There was no action found to process for {prop}.  Try creating a source generator that should create it to invoke");
                        result.IsValid = false;
                        continue;
                    }
                    PopulateAction<T>? populateAction = null;
                    if (populateActions is not null)
                    {
                        rets = populateActions.TryGetValue(prop, out populateAction);
                        if (rets && used.RuleCategory == EnumRuleCategory.Forbid)
                        {
                            result.ExtraMessages.Add($"The property was set to not even allow the rule to be set for property {prop}.  Try adjusting to allow the rule to be added, remove the rule or fix the source generator used");
                            result.IsValid = false;
                            continue;
                        }
                    }
                    if (populateAction is not null)
                    {
                        // Very much a .Rules() action
                        if (populateAction.ProhibitInStrictMode)
                        {
                            result.ExtraMessages.Add(
                               $"When StrictMode is set to True the PersonFaker.Rules(...) method cannot verify that all properties have rules. You need to use PersonFaker.RuleFor( x => x.Prop, ...) for each property to ensure each property has an associated rule when StrictMode is true; otherwise, set StrictMode to False in order to use PersonFaker.Rules() method.");
                            result.IsValid = false;
                        }
                    }
                    else //The user doesn't know about this property or field. Log it as a validation error.
                    {
                        result.MissingRules.Add(prop);
                        result.IsValid = false;
                    }
                }
            }
        }

        return result;
    }
    /// <summary>
    /// Provides implicit type conversion from <seealso cref="Faker{T}"/> to <typeparamref name="T"/>. IE: Order testOrder = faker;
    /// </summary>
    public static implicit operator T(Faker<T> faker)
    {
        return faker.Generate();
    }



    //this is needed so i can have the intellisense in visual studio.
    /// <summary>
    /// Not Implemented: This method only exists as a work around for Visual Studio IntelliSense.
    /// </summary>
    [Obsolete("This exists here only as a Visual Studio IntelliSense work around.", true)]
    public void RuleFor<TProperty>(Expression<Func<T, TProperty>> property)
    {
        throw new NotImplementedException();
    }
}