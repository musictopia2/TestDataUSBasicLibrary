using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.EventsFunctions<TestDataUSBasicLibrary.DataSets.Events>;
namespace TestDataUSBasicLibrary.DataSets;
public class Events : DataSet
{
    public string History()
    {
        return ee1.History(this);
    }
}