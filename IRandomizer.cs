namespace TestDataUSBasicLibrary;
public interface IRandomizer
{
    string AlphaNumeric(int length);
    bool Bool();
    bool Bool(int likelihood);
    char Char(char min = '\0', char max = '\uffff');
    BasicList<char> Chars(char min = '\0', char max = '\uffff', int count = 5);
    decimal Decimal(decimal min = 0.0M, decimal max = 1.0M);
    BasicList<int> Digits(int count, int minDigit = 0, int maxDigit = 9);
    double Double(double min = 0, double max = 1);
    int Even(int min = 0, int max = 1);
    float Float(float min = 0, float max = 1);
    Guid Guid();
    string Hash(int length = 40, bool upperCase = false);
    string Hexadecimal(int length = 1, string prefix = "0x");
    int Int(int min = int.MinValue, int max = int.MaxValue);
    T ListItem<T>(BasicList<T> list);
    IBasicList<T> ListItems<T>(BasicList<T> items, int? count = null);
    long Long(long min = long.MinValue, long max = long.MaxValue);
    int Number(int min = 0, int max = 1);
    int Number(int max);
    int Odd(int min = 0, int max = 1);
    string Replace(string format);
    string ReplaceNumbers(string format, char symbol = '#');
    short Short(short min = short.MinValue, short max = short.MaxValue);
    string String(int? length = null, char minChar = '\0', char maxChar = '\uffff');
    string String(int minLength, int maxLength, char minChar = '\0', char maxChar = '\uffff');
    string String2(int length, string chars = "abcdefghijklmnopqrstuvwxyz");
    string String2(int minLength, int maxLength, string chars = "abcdefghijklmnopqrstuvwxyz");
}