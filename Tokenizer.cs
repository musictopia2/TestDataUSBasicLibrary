namespace TestDataUSBasicLibrary;
public class Tokenizer
{
    public static BasicList<DataSetClassModel> DataSets { get; set; } = [];
    private static readonly char[] _separator = [','];
    static Tokenizer()
    {
        //for now.
        //RegisterMustashMethods(typeof(Faker));
    }
    private static void SetObjectsForFilters(BasicList<DataSetClassModel> filters, object[] dataSets, string className)
    {
        bool hadOne = false;
        foreach (var item1 in filters)
        {
            foreach (var item2 in dataSets)
            {
                if (item2.GetType() == item1.DeclaringType)
                {
                    item1.DataSet = item2;
                    hadOne = true;
                }
            }
        }
        if (hadOne == false)
        {
            throw new ArgumentException($"Can't parse {className} because the dataset was not provided in the {nameof(dataSets)} parameter.", nameof(dataSets));
        }
    }
    private static (DataSetMethodModel, object) GetMethod(BasicList<DataSetClassModel> filtered, string className, string methodName, BasicList<string> arguments)
    {
        BasicList<DataSetMethodModel> results = [];
        object? currentObject = null;
        foreach (var item1 in filtered)
        {
            foreach (var item2 in item1.Methods)
            {
                if (item2.Name.Equals(methodName, StringComparison.CurrentCultureIgnoreCase))
                {
                    currentObject ??= item1.DataSet;
                    results.Add(item2);
                }
            }
        }
        if (results.Count == 0)
        {
            throw new CustomBasicException($"No method was found with class of {className} and method {methodName}");
        }
        if (currentObject is null)
        {
            throw new CustomBasicException($"No object was captured with class of {className} and method {methodName}");
        }
        if (results.Count == 1)
        {
            return (results.Single(), currentObject); //if only one, return that one.
        }
        //could have overloaded though.
        //try to find the best match.  if none is found, then the first one is fine.
        foreach (var item in results)
        {
            if (item.TotalArgumentsCount == arguments.Count)
            {
                return (item, currentObject); //because the arugments match perfectly.
            }
        }
        //if the arguments don't match perfectly, then use the first one.
        return (results.First(), currentObject);
    }
    public static string Parse(string str, params object[] dataSets)
    {
        if (DataSets.Count == 0)
        {
            throw new CustomBasicException("There was no datasets.  Try creating a source generator to capture it.");
        }
        //Recursive base case. If there are no more {{ }} handle bars,
        //return.
        var start = str.IndexOf("{{", StringComparison.Ordinal);
        var end = str.IndexOf("}}", StringComparison.Ordinal);
        if (start == -1 && end == -1)
        {
            return str;
        }
        //We have some handlebars to process. Get the method name and arguments.
        ParseMustashText(str, start, end, out var className, out var methodName, out var arguments);
        BasicList<DataSetClassModel> filters = DataSets.Where(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase)).ToBasicList();
        if (filters.Count == 0)
        {
            throw new ArgumentException($"Class {className} was never registered.  Try creating a source generator");
        }
        SetObjectsForFilters(filters, dataSets, className);
        (DataSetMethodModel details, object currentObject) = GetMethod(filters, className, methodName, arguments);
        int required = details.TotalArgumentsCount - details.OptionalArgumentsCount;
        if (arguments.Count < required)
        {
            throw new ArgumentException($"Class {className} with method name {methodName} did not have enough arguments sent in");
        }
        if (arguments.Count > details.TotalArgumentsCount)
        {
            throw new ArgumentException($"Class {className} with method name {methodName} had too many arguments sent in");
        }
        string fakeVal = "";
        try
        {
            fakeVal = details.Invoke!(currentObject, arguments);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error when invoking the command.  Most likely the arguments are incorrect.", ex);
        }
        var sb = new StringBuilder();
        sb.Append(str, 0, start);
        sb.Append(fakeVal);
        sb.Append(str.AsSpan(end + 2));
        return Parse(sb.ToString(), dataSets);
    }
    private static void ParseMustashText(string str, int start, int end, out string className, out string methodName, out BasicList<string> arguments)
    {
        var methodCall = str.Substring(start + 2, end - start - 2)
           .Replace("}}", "")
           .Replace("{{", "");
        var argumentsStart = methodCall.IndexOf('(');
        string fullName;
        if (argumentsStart != -1)
        {
            var argumentsString = GetArgumentsString(methodCall, argumentsStart);
            fullName = methodCall.Substring(0, argumentsStart).Trim();
            arguments = argumentsString.Split(_separator, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToBasicList();
        }
        else
        {
            fullName = methodCall;
            arguments = [];
        }
        fullName = fullName.ToUpperInvariant();
        BasicList<string> items = fullName.Split('.').ToBasicList();
        if (items.Count != 2)
        {
            throw new CustomBasicException("Should only have 2 parts to capture class and method");
        }
        className = items.First();
        methodName = items.Last();
    }
    private static string GetArgumentsString(string methodCall, int parametersStart)
    {
        var parametersEnd = methodCall.IndexOf(')');
        if (parametersEnd == -1)
        {
            throw new ArgumentException($"The method call '{methodCall}' is missing a terminating ')' character.");
        }
        return methodCall.Substring(parametersStart + 1, parametersEnd - parametersStart - 1);
    }
}