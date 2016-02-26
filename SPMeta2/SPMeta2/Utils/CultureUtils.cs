using System;
using System.Globalization;
using System.Threading;

namespace SPMeta2.Utils
{
    public class CultureUtils
    {
        /// <summary>
        /// This methods allows code to be run in a specific thread culture.
        /// This can be necessary as a workaround to get/set some SSOM properties.
        /// </summary>
        /// <param name="culture">Culture to be temporarily set</param>
        /// <param name="action">action to be performed</param>
        /// <returns></returns>
        public static void WithCulture(CultureInfo culture, Action action)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            CultureInfo orgCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo orgUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                action();
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = orgCulture;
                Thread.CurrentThread.CurrentUICulture = orgUICulture;
            }
        }
    }
}
