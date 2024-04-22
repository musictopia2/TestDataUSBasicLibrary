using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.MusicFunctions<TestDataUSBasicLibrary.DataSets.Music>; //only needed here.
namespace TestDataUSBasicLibrary.DataSets;
public class Music : InternalDataSet
{
    protected override string Category => nameof(Music);

    /// <summary>
    /// Get a music genre
    /// </summary>
    public string Genre()
    {
        return ee1.Genre(this);
    }
}