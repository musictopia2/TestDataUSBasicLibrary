namespace TestDataUSBasicLibrary.CustomDataFunctions;
public static class NameFunctions<T> where T : Name
{
    public static Func<T, EnumGender> Gender { get; set; } = name =>
    {
        int ask = name.Random.Int(1, 2);
        if (ask == 1)
        {
            return EnumGender.Male;
        }
        return EnumGender.Female;
    };
    public static Func<T, string> JobTitle { get; set; } = name =>
    {
        BasicList<string> possibleTitles =
        [
            "Data Entry Clerk",
            "Accountant",
            "Doctor",
            "Lawyer",
            "Software Engineer",
            "Office Assistant",
            "Dishwasher",
            "Courtesy Clerk",
            "Security Guard",
            "Busboy",
            "Marketing Manager",
            "Police Officer",
            "Fire Fighter",
            "Preacher",
            "Secretary",
            "Receptionist",
            "Teacher",
            "Nurse",
            "Counselor",
            "Cook",
            "Bookkeeper",
            "Accounting Clerk",
            "Truck Driver",
            "Supervisor",
            "Chief Financial Officer",
            "Artist",
            "Actor",
            "Auditor",
            "Front Desk Clerk",
            "Payroll Clerk",
            "Interior Designer",
            "Video Editor",
            "Engineer",
            "Plumber",
            "Tutor",
            "Professor",
            "Flight Attendant",
            "Barista",
            "Journalist",
            "Therapist",
            "Chemist",
            "Probation Officer",
            "Caregiver",
            "Dentist",
            "Data Analyst",
            "Electrical Engineer",
            "Principal",
            "Carpenter",
            "Electrician",
            "Painter",
            "Cashier",
            "Animal Trainer",
            "Paralegal",
            "Fitness Instructor"
        ];
        return name.Random.ListItem(possibleTitles);
    };
    public static Func<T, EnumGender?, string> FirstName { get; set; } = (name, gender) =>
    {
        gender ??= name.Gender();
        BasicList<string> list;
        var customData = DataSet.GetCustomData;
        if (gender == EnumGender.Male)
        {
            list = customData.FirstNamesMale;
        }
        else if (gender == EnumGender.Female)
        {
            list = customData.FirstNamesFemale;
        }
        else
        {
            throw new CustomBasicException("No gender found when getting first name");
        }
        return name.Random.ListItem(list);
    };
    public static Func<T, string> LastName { get; set; } = name =>
    {
        var customData = DataSet.GetCustomData;
        var list = customData.LastNames;
        return name.Random.ListItem(list);
    };
    public static Func<T, EnumGender?, string> FullName { get; set; } = (name, gender) =>
    {
        string firsts = name.FirstName(gender);
        string lasts = name.LastName();
        return $"{firsts} {lasts}";
    };
    public static Func<T, EnumGender?, string> Prefix { get; set; } = (name, gender) =>
    {
        gender ??= name.Gender();
        BasicList<string> list;
        if (gender == EnumGender.Male)
        {
            list = [
                "Mr.",
                "Sr.",
                "Dr.",
                "Prof."
                ];
        }
        else if (gender == EnumGender.Female)
        {
            list = [
                "Mrs.",
                "Miss.",
                "Sr.",
                "Dr.",
                "Prof."
               ];
        }
        else
        {
            throw new CustomBasicException("Unable to get prefix because wrong gender");
        }
        return name.Random.ListItem(list);
    };
    public static Func<T, string> Suffix { get; set; } = name =>
    {
        BasicList<string> list =
            [
              "Jr.",
              "Sr.",
              "I",
              "II",
              "III",
              "IV",
              "V",
              "MD",
              "DDS",
              "PhD",
              "DVM"
            ];
        return name.Random.ListItem(list);
    };
    public static Func<T, string, string, bool?, bool?, EnumGender?, string> FindName { get; set; } =
        (name, firstName, lastName, withPrefix, withSuffix, gender) =>
        {
            gender ??= name.Gender();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                firstName = name.FirstName(gender);
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = name.LastName();
            }

            if (!withPrefix.HasValue && !withSuffix.HasValue)
            {
                withPrefix = name.Random.Bool(10);
                withSuffix = name.Random.Bool(30);
            }
            var prefix = withPrefix.GetValueOrDefault() ? name.Prefix(gender) : "";
            var suffix = withSuffix.GetValueOrDefault() ? name.Suffix() : "";
            return $"{prefix} {firstName} {lastName} {suffix}".Trim();
        };
    public static Func<T, string> Ssn { get; set; } = name =>
    {
        IRandomNumberList random = name.Random;
        return random.NextSSN();
    };
}