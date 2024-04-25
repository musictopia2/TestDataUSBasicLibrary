using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.NameFunctions<TestDataUSBasicLibrary.DataSets.Name>; //only needed here.
namespace TestDataUSBasicLibrary.DataSets;
public class Name : DataSet
{
    public EnumGender Gender()
    {
        return ee1.Gender(this);
    }
    /// <summary>
    /// Get a first name.
    /// </summary>
    /// <param name="gender">if sending in null, then picks gender at random</param>
    public string FirstName(EnumGender? gender = null) //they allow null so i guess i can allow null too (as default)
    {
        return ee1.FirstName(this, gender);
    }
    /// <summary>
    /// Get a last name.
    /// </summary>
    public string LastName()
    {
        return ee1.LastName(this);
    }
    /// <summary>
    /// Get a full name, concatenation of calling FirstName and LastName.
    /// </summary>
    public string FullName(EnumGender? gender = null)
    {
        return ee1.FullName(this, gender);
    }
    /// <summary>
    /// Gets a random prefix for a name.
    /// </summary>
    public string Prefix(EnumGender? gender = null)
    {
        return ee1.Prefix(this, gender);
    }
    /// <summary>
    /// Gets a random suffix for a name.
    /// </summary>
    public string Suffix()
    {
        return ee1.Suffix(this);
    }
    /// <summary>
    /// Gets a full name.
    /// </summary>
    /// <param name="firstName">Use this first name.</param>
    /// <param name="lastName">use this last name.</param>
    /// <param name="withPrefix">Add a prefix?</param>
    /// <param name="withSuffix">Add a suffix?</param>
    public string FindName(string firstName = "", string lastName = "", bool? withPrefix = null, bool? withSuffix = null, EnumGender? gender = null)
    {
        return ee1.FindName(this, firstName, lastName, withPrefix, withSuffix, gender);
    }
    /// <summary>
    /// gets social security number.
    /// </summary>
    /// <returns></returns>
    public string Ssn()
    {
        return ee1.Ssn(this);
    }
    public string JobTitle()
    {
        return ee1.JobTitle(this);
    }
}