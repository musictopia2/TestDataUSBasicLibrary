namespace TestDataUSBasicLibrary;
/// <summary>
/// Extensions for <see cref="Faker{T}"/>.
/// </summary>
public static class ExtensionsForFakerT
{
    extension<T>(T value)
        where T: class
    {
        /// <summary>
        /// Helpful extension for creating randomly null values for <seealso cref="Faker{T}"/>.RuleFor() rules.
        /// Example: .RuleFor(x=>x.Prop, f=>f.Random.Word().OrNull(f))
        /// </summary>
        /// <param name="f">The Faker facade. This is usually the f from RuleFor(.., f => lambda).</param>
        /// <param name="nullWeight">The probability of null occurring. Range [1.0f - 0.0f] (100% and 0%) respectively. For example, if 15% null is desired pass nullWeight = 0.15f.</param>
        public T OrNull(in Faker f, float nullWeight = 0.5f)
        {
            if (nullWeight > 1 || nullWeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nullWeight), $".{nameof(OrNull)}() {nameof(nullWeight)} of '{nullWeight}' must be between 1.0f and 0.0f.");
            }
            return f.Random.Float() > nullWeight ? value : null!;
        }
    }
    extension<T>(T value)
        where T: struct
    {
        /// <summary>
        /// Helpful extension for creating randomly null values for <seealso cref="Faker{T}"/>.RuleFor() rules.
        /// Example: .RuleFor(x=>x.Prop, f=>f.Random.Int().OrNull(f))
        /// </summary>
        /// <param name="f">The Faker facade. This is usually the f from RuleFor(.., f => lambda).</param>
        /// <param name="nullWeight">The probability of null occurring. Range [1.0f - 0.0f] (100% and 0%) respectively. For example, if 15% null is desired pass nullWeight = 0.15f.</param>
        public T? OrNull(Faker f, float nullWeight = 0.5f)
        {
            if (nullWeight > 1 || nullWeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nullWeight), $".{nameof(OrNull)}() {nameof(nullWeight)} of '{nullWeight}' must be between 1.0f and 0.0f.");
            }
            return f.Random.Float() > nullWeight ? new T?(value) : null;
        }
    }
    extension<T>(T value)
    {
        /// <summary>
        /// Helpful extension for creating randomly default(T) values for <seealso cref="Faker{T}"/>.RuleFor() rules.
        /// Example: .RuleFor(x=>x.Prop, f=>f.Random.Word().OrDefault(f))
        /// </summary>
        /// <param name="f">The Faker facade. This is usually the f from f => lambda.</param>
        /// <param name="defaultWeight">The probability of the default value occurring. Range [1.0f - 0.0f] (100% and 0%) respectively. For example, if 15% default(T) is desired pass defaultWeight = 0.15f.</param>
        /// <param name="defaultValue">The default value to return.</param>
        public T OrDefault(Faker f, float defaultWeight = 0.5f, T? defaultValue = default)
        {
            if (defaultWeight > 1 || defaultWeight < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(defaultWeight), $".{nameof(OrDefault)}() {nameof(defaultWeight)} of '{defaultWeight}' must be between 1.0f and 0.0f. ");
            }

            return f.Random.Float() > defaultWeight ? value : defaultValue!;
        }
    }   
}