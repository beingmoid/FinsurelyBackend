using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace PanoramaBackend.Api.Controllers
{
    public static class ExtentionMethod
    {
        public static void RemoveAll<T>(this ICollection<T> source,
                                  Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (predicate == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            source.Where(predicate).ToList().ForEach(e => source.Remove(e));
        }

        public static int ToInt(this object obj)

        {
            return Convert.ToInt32(obj);
        }
        public static DateTime ToDateTime(this string datetime, char dateSpliter = '-', char timeSpliter = ':', char millisecondSpliter = ',')

        {
            try
            {
                datetime = datetime.Trim();
                datetime = datetime.Replace("  ", " ");
                string[] body = datetime.Split(' ');
                string[] date = body[0].Split(dateSpliter);
                int year = Convert.ToInt32(date[0]); 
                int month = Convert.ToInt32(date[1]);
                int day = Convert.ToInt32(date[2]);
                int hour = 0, minute = 0, second = 0, millisecond = 0;
                if (body.Length == 2)
                {
                    string[] tpart = body[1].Split(millisecondSpliter);
                    string[] time = tpart[0].Split(timeSpliter);
                    hour =Convert.ToInt32(time[0]);
                    minute = Convert.ToInt32(time[1]);
                    if (time.Length == 3) second = Convert.ToInt32(time[2]);
                    if (tpart.Length == 2) millisecond = Convert.ToInt32(tpart[1]);
                }
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch
            {
                return new DateTime();
            }
        }
        //public static void RemoveAll<T>(this ICollection<T> source,
        //                           Func<T, bool> predicate)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException("source", "source is null.");

        //    if (predicate == null)
        //        throw new ArgumentNullException("predicate", "predicate is null.");

        //    source.Where(predicate).ToList().ForEach(e => source.Remove(e));
        //}
    }
}
