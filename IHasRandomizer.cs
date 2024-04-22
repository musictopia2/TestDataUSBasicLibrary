namespace TestDataUSBasicLibrary;
/// <summary>
/// Objects should implement this interface if they use a
/// <see cref="Randomizer"/>.
/// </summary>
public interface IHasRandomizer
{
    /// <summary>
    /// Access the randomizer on the implementing object. When the property value
    /// is set, the object is instructed to use the randomizer as a source of generating
    /// random values. Additionally, setting this property also notifies any dependent
    /// via <see cref="SeedNotifier.Notify"/>. 
    /// </summary>
    Randomizer Random { set; }

    /// <summary>
    /// Retrieves the internal notifier registry for this object.
    /// </summary>
    SeedNotifier GetNotifier();
}