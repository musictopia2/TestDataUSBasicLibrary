namespace TestDataUSBasicLibrary.WordSpecializedClasses;
public class WordModel //hopefully can still deserialize (?)
{
    public string Word { get; set; } = "";
    public EnumInflectionEnding Inflection { get; set; } = EnumInflectionEnding.None;
    public bool GoodUserName { get; set; }
    public EnumSpecialCategory SpecialCategory { get; set; } = EnumSpecialCategory.None;
}