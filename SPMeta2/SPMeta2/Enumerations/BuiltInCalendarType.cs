using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Builtin SharePoint calendar types.
    /// Reflects Microsoft.SharePoint.SPCalendarType enum.
    /// </summary>
    public static class BuiltInCalendarType
    {
        #region properties

        public static string None = "None";
        public static string Gregorian = "Gregorian";
        public static string Japan = "Japan";
        public static string Taiwan = "Taiwan";
        public static string Korea = "Korea";

        public static string Hijri = "Hijri";
        public static string Thai = "Thai";
        public static string Hebrew = "Hebrew";
        public static string GregorianMEFrench = "GregorianMEFrench";
        public static string GregorianArabic = "GregorianArabic";

        public static string GregorianXLITEnglish = "GregorianXLITEnglish";
        public static string GregorianXLITFrench = "GregorianXLITFrench";

        public static string KoreaJapanLunar = "KoreaJapanLunar";
        public static string ChineseLunar = "ChineseLunar";

        public static string SakaEra = "SakaEra";
        public static string UmAlQura = "UmAlQura";

        #endregion
    }
}
