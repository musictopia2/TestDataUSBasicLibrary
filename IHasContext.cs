namespace TestDataUSBasicLibrary;
/// <summary>
/// Marker interface for objects that have a context storage property.
/// </summary>
public interface IHasContext
{
    Dictionary<string, object> Context { get; }
}