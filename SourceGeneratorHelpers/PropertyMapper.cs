namespace TestDataUSBasicLibrary.SourceGeneratorHelpers;
public class PropertyMapper<T>
    where T : class
{
    //since i am doing as dictionary, then go ahead and not worry about name here.
    public EnumRuleCategory RuleCategory { get; set; } = EnumRuleCategory.Default;
    public Type? Type { get; set; }
    public Action<T, object>? Action { get; set; } //this means if not filled in, then needs to raise error.
}