namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class EventsFunctions<T>
    where T: Events
{
    public static Func<T, string> History { get; set; } = events =>
    {
        BasicList<string> list =
        [
            "Wright brothers make the first powered flight",
            "World War I begins",
            "Treaty of Versailles ends World War I",
            "Wall Street Crash triggers the Great Depression",
            "World War II begins",
            "Atomic bombs dropped on Hiroshima and Nagasaki",
            "India gains independence from British rule",
            "Korean War begins",
            "Brown v. Board of Education ends racial segregation in US schools",
            "Yuri Gagarin becomes the first human in space",
            "Martin Luther King Jr.'s 'I Have a Dream' speech",
            "Apollo 11 Moon Landing",
            "Intel releases the first microprocessor",
            "Berlin Wall falls",
            "USSR dissolves",
            "Nelson Mandela becomes South Africa's first black president",
            "Hong Kong returns to Chinese rule",
            "September 11 attacks in the US",
            "Global financial crisis",
            "Deepwater Horizon oil spill",
            "Arab Spring uprisings",
            "Edward Snowden leaks NSA documents",
            "Paris Agreement on climate change",
            "Brexit referendum",
            "COVID-19 pandemic begins",
            "SpaceX Crew-2 mission launches",
            "TBD"
        ];
        return events.Random.ListItem(list);
    };
}