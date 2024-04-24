namespace TestDataUSBasicLibrary.DataSets;
public class Words : DataSet
{
    private readonly BasicList<WordModel> _words = [];
    public Words()
    {
        string text = rr1.CompleteWords.GetJsonText();
        _words = jj1.DeserializeObject<BasicList<WordModel>>(text);
    }
    public string AccountName(int minWords = 2, int maxWords = 2)
    {
        //default will have 2 to 2 words.
        var list = _words.Where(x => x.GoodUserName).ToBasicList();
        if (maxWords > minWords)
        {
            throw new CustomBasicException("Cannot have maximum words less than minimum words");
        }
        int number = Random.Number(minWords, maxWords);
        var filters = Random.ListItems(list, number).Select(x => x.Word);
        return string.Join("", filters);
    }
    public string Food()
    {
        return SpecialWord(EnumSpecialCategory.Food);
    }
    public string Drink()
    {
        return SpecialWord(EnumSpecialCategory.Drink);
    }
    public string Animal()
    {
        return SpecialWord(EnumSpecialCategory.Animals);
    }
    public string Nature()
    {
        return SpecialWord(EnumSpecialCategory.Nature);
    }
    public string Financial()
    {
        return SpecialWord(EnumSpecialCategory.Financial);
    }
    public string SpecialWord(EnumSpecialCategory category)
    {
        var list = _words.Where(x => x.SpecialCategory == category).ToBasicList();
        return Random.ListItem(list).Word;
    }
    public string RegularWord()
    {
        var list = _words.Where(x => x.Inflection == EnumInflectionEnding.None).ToBasicList();
        return Random.ListItem(list).Word;
    }
    public string EndInflectionWord(EnumInflectionEnding ending)
    {
        var list = _words.Where(x => x.Inflection == ending).ToBasicList();
        return Random.ListItem(list).Word;
    }
    public string AnyWord()
    {
        return Random.ListItem(_words).Word;
    }
    public string CustomWord(Func<WordModel, bool> predicate) //so you can decide what you want here.
    {
        var list = _words.Where(predicate).ToBasicList();
        return Random.ListItem(list).Word;
    }
    //i don't think i should use this for several words (except for user names).
}