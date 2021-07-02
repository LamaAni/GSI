using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Coading
{
    public static class Extentions
    {
        /// <summary>
        /// Returns a string resource.
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static string GetStringResource(this Assembly asm, string resourceName)
        {
            string rslt = "";
            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                rslt = reader.ReadToEnd();
            }
            return rslt;
        }
    }
}

public static class GLOBAL_CODING_EXTEND
{
    /// <summary>
    /// Executes a foreach function for the array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="col"></param>
    /// <param name="f"></param>
    public static void Foreach<T>(this IEnumerable<T> col, Action<T> f)
    {
        foreach (T val in col.ToArray())
            f(val);
    }

    /// <summary>
    /// Returns the index of the first match or -1.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="col"></param>
    /// <param name="predict"></param>
    /// <returns></returns>
    public static int IndexOfFirst<T>(this IEnumerable<T> col, Func<T, bool> predict)
    {
        int i = 0;
        foreach (T v in col)
        {
            if (predict(v))
                return i;
            i += 1;
        }
        return -1;
    }

    public static string ToShortTimespanString(this TimeSpan span)
    {
        return (span.Days > 0 ? span.TotalDays.ToString("#00") + ":" : "") + span.Hours.ToString("00") + ":"
            + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00") + "."
            + (span.Milliseconds / 100).ToString("0");
    }

    public static bool IsPowerOf2(this int x)
    {
        return (x != 0) && ((x & (x - 1)) == 0);
    }

}
