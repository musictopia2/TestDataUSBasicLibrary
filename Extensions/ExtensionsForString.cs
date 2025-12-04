namespace TestDataUSBasicLibrary.Extensions;

/// <summary>
/// General helper string extensions for working with fake data.
/// </summary>
public static class ExtensionsForString
{
    extension (string payLoad)
    {
        /// <summary>
        /// Clamps the length of a string filling between min and max characters.
        /// If the string is below the minimum, the string is appended with paddingChar up to the minimum length.
        /// If the string is over the maximum, the string is truncated at maximum characters; additionally, if the result string ends with
        /// whitespace, it is replaced with a paddingChar characters.
        /// </summary>
        public string ClampLength(int? min = null, int? max = null, char paddingChar = 'A')
        {
            if (max != null && payLoad.Length > max)
            {
                payLoad = payLoad.Substring(0, max.Value).Trim();
            }
            if (min != null && min > payLoad.Length)
            {
                var missingChars = min - payLoad.Length;
                var fillerChars = "".PadRight(missingChars.Value, paddingChar);
                return payLoad + fillerChars;
            }
            return payLoad;
        }

        /// <summary>
        /// A string extension method that removes the diacritics character from the strings.
        /// </summary>
        /// <returns>The string without diacritics character.</returns>
        public string RemoveDiacritics()
        {
            string normalizedString = payLoad.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char t in normalizedString)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
    
}