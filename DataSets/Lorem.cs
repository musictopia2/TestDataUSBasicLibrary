using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.LoremFunctions<TestDataUSBasicLibrary.DataSets.Lorem>; //only needed here.
namespace TestDataUSBasicLibrary.DataSets;
public class Lorem : InternalDataSet
{
    protected override string Category => nameof(Lorem);
    /// <summary>
    /// Get a random lorem word.
    /// </summary>
    public string Word()
    {
        return ee1.Word(this);
    }
    /// <summary>
    /// Get an list of random lorem words.
    /// </summary>
    /// <param name="num">The number of random lorem words to return.</param>
    public BasicList<string> Words(int num = 3)
    {
        return Enumerable.Range(1, num).Select(_ => Word()).ToBasicList();
    }
    /// <summary>
    /// Get a character letter.
    /// </summary>
    /// <param name="num">The number of characters to return.</param>
    public string Letter(int num = 1)
    {
        if (num <= 0)
        {
            return string.Empty;
        }
        var words = Words(1)[0];
        var characters = Random.ListItem(words.ToBasicList());
        return characters + Letter(num - 1);
    }
    /// <summary>
    /// Get a random sentence of specific number of words. 
    /// </summary>
    /// <param name="wordCount">Get a sentence with wordCount words. Defaults between 3 and 10.</param>
    /// <param name="range">Add anywhere between 0 to 'range' additional words to wordCount. Default is 0.</param>
    public string Sentence(int? wordCount = null, int? range = 0)
    {
        var wc = wordCount ?? Random.Number(3, 10);
        if (range > 0)
        {
            wc += Random.Number(range.Value);
        }

        var sentence = string.Join(" ", Words(wc));
        return string.Concat(sentence.Substring(0, 1).ToUpper(), sentence.AsSpan(1), ".");
    }

    /// <summary>
    /// Get some sentences.
    /// </summary>
    /// <param name="sentenceCount">The number of sentences.</param>
    /// <param name="separator">The string to separate sentences.</param>
    public string Sentences(int? sentenceCount = null, string separator = "\n")
    {
        var sc = sentenceCount ?? Random.Number(2, 6);
        var sentences = Enumerable.Range(1, sc)
           .Select(_ => Sentence());
        return string.Join(separator, sentences);
    }
    /// <summary>
    /// Get a paragraph.
    /// </summary>
    /// <param name="min">The minimum number of sentences in the paragraph.
    /// The final number of sentences returned in the paragraph is bound between [min, min + 3], inclusive.
    /// If you want an exact number of sentences, use the <seealso cref="Sentences"/> method.</param>
    public string Paragraph(int min = 3)
    {
        return Sentences(min + Random.Number(3), " ");
    }

    /// <summary>
    /// Get a specified number of paragraphs.
    /// </summary>
    /// <param name="count">Number of paragraphs.</param>
    /// <param name="separator">The string to separate paragraphs.</param>
    public string Paragraphs(int count = 3, string separator = "\n\n")
    {
        var paragraphs = Enumerable.Range(1, count)
           .Select(_ => Paragraph());
        return string.Join(separator, paragraphs);
    }

    /// <summary>
    /// Get a random number of paragraphs between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="min">Minimum number of paragraphs.</param>
    /// <param name="max">Maximum number of paragraphs.</param>
    /// <param name="separator">The string to separate the paragraphs.</param>
    public string Paragraphs(int min, int max, string separator = "\n\n")
    {
        var count = Random.Number(min, max);
        return Paragraphs(count, separator);
    }

    /// <summary>
    /// Get random text on a random lorem methods.
    /// </summary>
    public string Text()
    {
        BasicList<Func<string>> methods =
            [
                Word,
                () => Sentence(),
                () => Sentences(),
                () => Paragraph()
            ];
        var randomLoremMethod = Random.ListItem(methods);
        return randomLoremMethod();
    }

    /// <summary>
    /// Get lines of lorem.
    /// </summary>
    /// <param name="lineCount">The amount of lines to generate. Defaults between 1 and 5.</param>
    /// <param name="separator">The string to separate the lines.</param>
    public string Lines(int? lineCount = null, string separator = "\n")
    {
        var lc = lineCount ?? Random.Number(1, 5);
        return Sentences(lc, separator);
    }
    /// <summary>
    /// Slugify lorem words.
    /// </summary>
    /// <param name="wordcount">The amount of words to slugify.</param>
    public string Slug(int wordcount = 3)
    {
        var words = Words(wordcount);
        return Utils.Slugify(string.Join(" ", words));
    }
}