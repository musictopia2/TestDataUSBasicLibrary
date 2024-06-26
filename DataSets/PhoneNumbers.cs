﻿namespace TestDataUSBasicLibrary.DataSets;
public class PhoneNumbers : InternalDataSet
{
    protected override string Category => "phone_number"; //this is exception.
    /// <summary>
    /// Get a phone number.
    /// </summary>
    /// <param name="format">
    /// Format of phone number in any format.
    /// Replaces # characters with numbers. IE: '###-###-####' or '(###) ###-####'.
    /// </param>
    /// <returns>A random phone number.</returns>
    public string PhoneNumber(string? format = null)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            format = PhoneFormat();
        }
        return Random.Replace(ReplaceExclamChar(format));
    }

    /// <summary>
    /// Gets a phone number based on the locale's phone_number.formats[] array index.
    /// </summary>
    /// <param name="phoneFormatsArrayIndex">The array index as defined in the locale's phone_number.formats[] array.</param>
    /// <returns>A random phone number.</returns>
    public string PhoneNumberFormat(int phoneFormatsArrayIndex = 0)
    {
        var formatList = GetCustomList("formats");
        var format = (string)formatList![phoneFormatsArrayIndex];
        return PhoneNumber(format);
    }

    /// <summary>
    /// Gets the format of a phone number.
    /// </summary>
    /// <returns>A random phone number format.</returns>
    protected virtual string PhoneFormat()
    {
        return GetRandomListItem("formats");
    }

    /// <summary>
    /// Replaces special ! characters in phone number formats.
    /// </summary>
    /// <returns>The newly formed string.</returns>
    protected virtual string ReplaceExclamChar(string s)
    {
        return Randomizer.ReplaceSymbols(s, '!', () => Convert.ToChar('0' + Random.Number(2, 9)));
    }
}