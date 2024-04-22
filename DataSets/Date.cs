using ee1 = TestDataUSBasicLibrary.CustomDataFunctions.DateFunctions<TestDataUSBasicLibrary.DataSets.Date>;
namespace TestDataUSBasicLibrary.DataSets;
public class Date : InternalDataSet
{
    protected override string Category => nameof(Date);
    protected internal DateTime GetTimeReference() => LocalSystemClock?.Invoke() ?? throw new CustomBasicException("Did not set a time reference.  If you really want to use the system clock, you have to set manually");
    /// <summary>
    /// Sets the system clock time the testing framework uses for date calculations.
    /// This value is normally <seealso cref="DateTime.Now"/>. If deterministic times are desired,
    /// set the <seealso cref="LocalSystemClock"/> to a single instance in time.
    /// IE: () => new DateTime(2018, 4, 23);
    /// Setting this value will only effect and only apply to this single Date instance.
    /// </summary>
    /// <example>() => new DateTime(2018, 4, 23)</example>
    public Func<DateTime>? LocalSystemClock = null;

    /// <summary>
    /// Get a <see cref="DateTime"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
    /// </summary>
    /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
    public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
    {
        var maxDate = refDate ?? GetTimeReference();

        var minDate = maxDate.AddYears(-yearsToGoBack);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return maxDate - partTimeSpan;
    }

    /// <summary>
    /// Get a <see cref="DateTimeOffset"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
    /// </summary>
    /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
    public DateTimeOffset PastOffset(int yearsToGoBack = 1, DateTimeOffset? refDate = null)
    {
        var maxDate = refDate ?? GetTimeReference();

        var minDate = maxDate.AddYears(-yearsToGoBack);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return maxDate - partTimeSpan;
    }

    /// <summary>
    /// Gets an random timespan from ticks.
    /// </summary>
    protected internal TimeSpan RandomTimeSpanFromTicks(long totalTimeSpanTicks)
    {
        //find % of the timespan
        var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;
        return TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));
    }

    /// <summary>
    /// Get a <see cref="DateTime"/> that will happen soon.
    /// </summary>
    /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
    public DateTime Soon(int days = 1, DateTime? refDate = null)
    {
        var dt = refDate ?? GetTimeReference();
        return Between(dt, dt.AddDays(days));
    }

    /// <summary>
    /// Get a <see cref="DateTimeOffset"/> that will happen soon.
    /// </summary>
    /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
    public DateTimeOffset SoonOffset(int days = 1, DateTimeOffset? refDate = null)
    {
        var dt = refDate ?? GetTimeReference();
        return BetweenOffset(dt, dt.AddDays(days));
    }

    /// <summary>
    /// Get a <see cref="DateTime"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
    /// </summary>
    /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
    public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
    {
        var minDate = refDate ?? GetTimeReference();

        var maxDate = minDate.AddYears(yearsToGoForward);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return minDate + partTimeSpan;
    }

    /// <summary>
    /// Get a <see cref="DateTimeOffset"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
    /// </summary>
    /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
    public DateTimeOffset FutureOffset(int yearsToGoForward = 1, DateTimeOffset? refDate = null)
    {
        var minDate = refDate ?? GetTimeReference();

        var maxDate = minDate.AddYears(yearsToGoForward);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return minDate + partTimeSpan;
    }

    /// <summary>
    /// Get a random <see cref="DateTime"/> between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Start time - The returned <seealso cref="DateTimeKind"/> is used from this parameter.</param>
    /// <param name="end">End time</param>
    public DateTime Between(DateTime start, DateTime end)
    {
        var minTicks = Math.Min(start.Ticks, end.Ticks);
        var maxTicks = Math.Max(start.Ticks, end.Ticks);

        var totalTimeSpanTicks = maxTicks - minTicks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return new DateTime(minTicks, start.Kind) + partTimeSpan;
    }

    /// <summary>
    /// Get a random <see cref="DateTimeOffset"/> between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Start time - The returned <seealso cref="DateTimeOffset"/> offset value is used from this parameter</param>
    /// <param name="end">End time</param>
    public DateTimeOffset BetweenOffset(DateTimeOffset start, DateTimeOffset end)
    {
        var minTicks = Math.Min(start.Ticks, end.Ticks);
        var maxTicks = Math.Max(start.Ticks, end.Ticks);

        var totalTimeSpanTicks = maxTicks - minTicks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return new DateTimeOffset(minTicks, start.Offset) + partTimeSpan;
    }

    /// <summary>
    /// Get a random <see cref="DateTime"/> within the last few days.
    /// </summary>
    /// <param name="days">Number of days to go back.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
    public DateTime Recent(int days = 1, DateTime? refDate = null)
    {
        var maxDate = refDate ?? GetTimeReference();

        var minDate = days == 0 ? maxDate.Date : maxDate.AddDays(-days);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return maxDate - partTimeSpan;
    }

    /// <summary>
    /// Get a random <see cref="DateTimeOffset"/> within the last few days.
    /// </summary>
    /// <param name="days">Number of days to go back.</param>
    /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
    public DateTimeOffset RecentOffset(int days = 1, DateTimeOffset? refDate = null)
    {
        var maxDate = refDate ?? GetTimeReference();

        var minDate = days == 0 ? maxDate.Date : maxDate.AddDays(-days);

        var totalTimeSpanTicks = (maxDate - minDate).Ticks;

        var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

        return maxDate - partTimeSpan;
    }

    /// <summary>
    /// Get a random <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="maxSpan">Maximum of time to span. Default 1 week/7 days.</param>
    public TimeSpan Timespan(TimeSpan? maxSpan = null)
    {
        var span = maxSpan ?? TimeSpan.FromDays(7);

        return RandomTimeSpanFromTicks(span.Ticks);
    }

    /// <summary>
    /// Get a random month.
    /// </summary>
    public string Month(bool abbreviation = false)
    {
        string type;
        if (abbreviation)
        {
            type = "abbr";
        }
        else
        {
            type = "wide";
        }
        return GetRandomListItem($"month.{type}");
    }

    /// <summary>
    /// Get a random weekday.
    /// </summary>
    public string Weekday(bool abbreviation = false)
    {
        string type;
        if (abbreviation)
        {
            type = "abbr";
        }
        else
        {
            type = "wide";
        }
        return GetRandomListItem($"weekday.{type}");
    }

    /// <summary>
    /// Get a timezone string. Eg: Pacific, Mountain, etc.
    /// </summary>
    public string TimeZoneString()
    {
        return ee1.TimeZoneString(this);
    }


    /// <summary>
    /// Get a random <see cref="DateOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Start date</param>
    /// <param name="end">End date</param>
    public DateOnly BetweenDateOnly(DateOnly start, DateOnly end)
    {
        var maxDay = Math.Max(start.DayNumber, end.DayNumber);
        var minDay = Math.Min(start.DayNumber, end.DayNumber);

        var someDayNumber = this.Random.Number(minDay, maxDay);

        var dateBetween = DateOnly.FromDayNumber(someDayNumber);
        return dateBetween;
    }

    /// <summary>
    /// Get a <see cref="DateOnly"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
    /// </summary>
    /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
    public DateOnly PastDateOnly(int yearsToGoBack = 1, DateOnly? refDate = null)
    {
        var start = refDate ?? DateOnly.FromDateTime(GetTimeReference());
        var maxBehind = start.AddYears(-yearsToGoBack);

        return BetweenDateOnly(maxBehind, start);
    }

    /// <summary>
    /// Get a <see cref="DateOnly"/> that will happen soon.
    /// </summary>
    /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
    /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
    public DateOnly SoonDateOnly(int days = 1, DateOnly? refDate = null)
    {
        var start = refDate ?? DateOnly.FromDateTime(GetTimeReference());
        var maxForward = start.AddDays(days);

        return BetweenDateOnly(start, maxForward);
    }

    /// <summary>
    /// Get a <see cref="DateOnly"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
    /// </summary>
    /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
    /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
    public DateOnly FutureDateOnly(int yearsToGoForward = 1, DateOnly? refDate = null)
    {
        var start = refDate ?? DateOnly.FromDateTime(GetTimeReference());
        var maxForward = start.AddYears(yearsToGoForward);

        return BetweenDateOnly(start, maxForward);
    }

    /// <summary>
    /// Get a random <see cref="DateOnly"/> within the last few days.
    /// </summary>
    /// <param name="days">Number of days to go back.</param>
    /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
    public DateOnly RecentDateOnly(int days = 1, DateOnly? refDate = null)
    {
        var start = refDate ?? DateOnly.FromDateTime(GetTimeReference());
        var maxBehind = start.AddDays(-days);

        return BetweenDateOnly(maxBehind, start);
    }

    /// <summary>
    /// Get a random <see cref="TimeOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Start time</param>
    /// <param name="end">End time</param>
    public TimeOnly BetweenTimeOnly(TimeOnly start, TimeOnly end)
    {
        var diff = end - start;
        var diffTicks = diff.Ticks;

        var part = RandomTimeSpanFromTicks(diffTicks);

        return start.Add(part);
    }

    /// <summary>
    /// Get a <see cref="TimeOnly"/> that will happen soon.
    /// </summary>
    /// <param name="mins">Minutes no more than <paramref name="mins"/> ahead.</param>
    /// <param name="refTime">The time to start calculations. Default is time from <see cref="DateTime.Now"/>.</param>
    public TimeOnly SoonTimeOnly(int mins = 60, TimeOnly? refTime = null)
    {
        var start = refTime ?? TimeOnly.FromDateTime(GetTimeReference());
        var maxForward = start.AddMinutes(mins);

        return BetweenTimeOnly(start, maxForward);
    }

    /// <summary>
    /// Get a random <see cref="TimeOnly"/> within the last few Minutes.
    /// </summary>
    /// <param name="mins">Minutes <paramref name="mins"/> of the day to go back.</param>
    /// <param name="refTime">The Time to start calculations. Default is time from <see cref="DateTime.Now"/>.</param>
    public TimeOnly RecentTimeOnly(int mins = 60, TimeOnly? refTime = null)
    {
        var start = refTime ?? TimeOnly.FromDateTime(GetTimeReference());
        var maxBehind = start.AddMinutes(-mins);

        return BetweenTimeOnly(maxBehind, start);
    }
}