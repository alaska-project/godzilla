using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Extensions
{
    internal static class StringExtensions
    {
        public static string ToTitleCase(this string phrase)
        {
            string[] splittedPhrase = phrase.Split(' ', '-', '.');
            var sb = new StringBuilder();
            foreach (String s in splittedPhrase)
            {
                char[] splittedPhraseChars = s.ToCharArray();
                if (splittedPhraseChars.Length > 0)
                {
                    splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper().ToCharArray())[0];
                }
                sb.Append(new String(splittedPhraseChars));
            }
            return sb.ToString();
        }

        public static string ToCamelCase(this string phrase)
        {
            var t = phrase.ToTitleCase();
            return t.Length > 0 ? string.Concat(Char.ToLower(t[0]), t.Substring(1)) : string.Empty;
        }

        public static bool IEquals(this string value, string other)
        {
            return other != null && value?.ToLower() == other.ToLower();
        }

        public static IEnumerable<string> Split(this string value, string separator)
        {
            return value.Split(new string[] { separator }, StringSplitOptions.None);
        }

        public static string Trim(this string value, string trimString)
        {
            return value.TrimStart(trimString).TrimEnd(trimString);
        }

        public static string TrimStart(this string value, string trimString)
        {
            return value.TrimStart(trimString, StringComparison.CurrentCulture);
        }

        public static string TrimStart(this string value, string trimString, StringComparison comparsion)
        {
            if (value.StartsWith(trimString, comparsion))
                return value.Substring(trimString.Length);
            return value;
        }

        public static string TrimEnd(this string value, string trimString)
        {
            return value.TrimEnd(trimString, StringComparison.CurrentCulture);
        }

        public static string TrimEnd(this string value, string trimString, StringComparison comparsion)
        {
            if (value.EndsWith(trimString, comparsion))
                return value.Substring(0, value.Length - trimString.Length);
            return value;
        }

        public static string ReplaceStart(this string value, string pattern, string replaceWith, StringComparison comparsion)
        {
            if (value.StartsWith(pattern, comparsion))
                return string.Concat(replaceWith, value.TrimStart(pattern, comparsion));
            return value;
        }

        public static string ReplaceEnd(this string value, string pattern, string replaceWith, StringComparison comparsion)
        {
            if (value.EndsWith(pattern, comparsion))
                return string.Concat(replaceWith, value.TrimEnd(pattern, comparsion));
            return value;
        }

        public static string ReplaceLastSegment(this string value, string separator, string newSegment)
        {
            var hasEndingSeparator = value.EndsWith(separator);
            var segments = value.TrimEnd(separator).Split(separator).ToList();
            if (segments.Count == 0)
                return value;

            segments.ReplaceLast(newSegment);
            var newValue = string.Join(separator, segments);
            if (hasEndingSeparator)
                newValue += separator;
            return newValue;
        }

        public static string AppendSegment(this string value, string separator, string newSegment)
        {
            var hasEndingSeparator = value.EndsWith(separator);
            if (!hasEndingSeparator)
                value += separator;
            value += newSegment;
            if (hasEndingSeparator)
                value += separator;
            return value;
        }

        public static string GetLastSegment(this string value, string separator)
        {
            var hasEndingSeparator = value.EndsWith(separator);
            var segments = value.TrimEnd(separator).Split(separator).ToList();
            if (segments.Count == 0)
                throw new InvalidOperationException("No segments inside string");
            return segments.Last();
        }

        public static string EnsurePrefix(this string value, string prefix)
        {
            return value.StartsWith(prefix) ?
                value :
                string.Concat(prefix, value);
        }

        public static string EnsureSuffix(this string value, string suffix)
        {
            return value.EndsWith(suffix) ?
                value :
                string.Concat(value, suffix);
        }

        public static string LastSegment(this string value, string separator)
        {
            return value
                .Split(separator)
                .Last(x => !string.IsNullOrEmpty(x));
        }

        public static string FirstSegment(this string value, string separator)
        {
            return value
                .Split(separator)
                .First(x => !string.IsNullOrEmpty(x));
        }

        public static string RemoveLastSegment(this string value, string separator)
        {
            var segments = value.Split(separator).ToList();
            var indexToRemove = segments.FindLastIndex(x => !string.IsNullOrEmpty(x));
            segments.RemoveAt(indexToRemove);
            return string.Join(separator, segments);
        }

        public static string RemoveFirstSegment(this string value, string separator)
        {
            var segments = value.Split(separator).ToList();
            var indexToRemove = segments.FindIndex(x => !string.IsNullOrEmpty(x));
            segments.RemoveAt(indexToRemove);
            return string.Join(separator, segments);
        }
    }
}
