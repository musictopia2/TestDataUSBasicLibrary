namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class FinanceFunctions<T>
    where T: Finance
{
    public static Func<T, string> AccountType { get; set; } = obj =>
    {
        var type = obj.GetRandomListItem("account_type");
        return $"{type} Account";
    };
    public static Func<T, string> TransactionType { get; set; } = obj =>
    {
        var type = obj.GetRandomListItem("transaction_type");
        return $"{type} Account";
    };
    public static Func<T, string?, string> CreditCardNumber { get; set; } = (obj, cardType) =>
    {
        IRandomNumberList random = obj.Random;
        return random.NextCreditCardNumber().ToString(cardType);
    };

}