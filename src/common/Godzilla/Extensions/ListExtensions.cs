using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Extensions
{
    internal static class ListExtensions
    {
        public static void ReplaceLast<T>(this List<T> list, T newValue)
        {
            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
                list.Add(newValue);
            }
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count > 0)
                list.RemoveAt(list.Count - 1);
        }

        public static void RemoveFirst<T>(this List<T> list)
        {
            if (list.Count > 0)
                list.RemoveAt(0);
        }
    }
}
