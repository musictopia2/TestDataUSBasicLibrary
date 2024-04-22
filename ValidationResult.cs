namespace TestDataUSBasicLibrary;
/// <summary>
/// Contains validation results after validation
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// True if is valid
    /// </summary>
    internal bool IsValid { get; set; }

    /// <summary>
    /// A complete list of missing rules
    /// </summary>
    internal HashSet<string> MissingRules { get; } = [];

    /// <summary>
    /// Extra validation messages to display
    /// </summary>
    internal BasicList<string> ExtraMessages { get; } = [];
}