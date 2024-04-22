namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class CommerceFunctions<T>
    where T : Commerce
{
    public static Func<T, int, bool, string> Department { get; set; } = (obj, max, returnMax) =>
    {
        var num = max;
        if (!returnMax)
        {
            num = obj.Random.Number(1, max);
        }
        var cats = obj.Categories(num);
        if (num > 1)
        {
            var catJoin = string.Join(", ", cats.Take(cats.Count - 1)); //may have to be minus 1 (?)
            var catLast = cats.Last();
            return $"{catJoin} & {catLast}";
        }
        return cats[0];
    };
    public static Func<T, int, IBasicList<string>> Categories { get; set; } = (obj, num) =>
    {
        return obj.GetRandomListCollection("department", num);
    };
    public static Func<T, string> ProductName { get; set; } = obj =>
    {
        return $"{obj.ProductAdjective()} {obj.ProductMaterial()} {obj.Product()}";
    };
    public static Func<T, string> Product { get; set; } = obj =>
    {
        return obj.GetRandomListItem("product_name.product");
    };
    public static Func<T, string> ProductAdjective { get; set; } = obj =>
    {
        return obj.GetRandomListItem("product_name.adjective");
    };
    public static Func<T, string> ProductMaterial { get; set; } = obj =>
    {
        return obj.GetRandomListItem("product_name.material");
    };
}