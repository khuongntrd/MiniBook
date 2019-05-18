using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MiniBook.Strings
{
    internal class Localization
    {
        public static string CurrentLanguage { get; set; } = "vi";

        public static string GetString(string resourceName, string defaultValue = null)
        {
            var value = MiniBook.Strings.Resources.ResourceManager
                .GetString(resourceName, new CultureInfo(CurrentLanguage));

            return !string.IsNullOrEmpty(value) ? value : defaultValue;
        }
    }
}
