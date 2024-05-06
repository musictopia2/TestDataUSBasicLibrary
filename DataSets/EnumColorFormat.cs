namespace TestDataUSBasicLibrary.DataSets;
public enum EnumColorFormat
{
    /// <summary>
    /// Hexadecimal format: #4d0e68
    /// </summary>
    Hex = 1, //trying to make it 1 since the source generators can't pick up otherwise.
    /// <summary>
    /// CSS format: rgb(77,14,104)
    /// </summary>
    Rgb,
    /// <summary>
    /// Delimited R,G,B: 77,14,104
    /// </summary>
    Delimited
}