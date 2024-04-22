namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class MusicFunctions<T> where T : Music
{
    public static Func<T, string> Genre { get; set; } = music => { return music.GetRandomListItem("genre"); 
    };
}