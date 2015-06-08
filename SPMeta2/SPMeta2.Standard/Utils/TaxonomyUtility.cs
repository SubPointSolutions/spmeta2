using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Standard.Utils
{
    public static class TaxonomyUtility
    {
        #region static

        static TaxonomyUtility()
        {
            InitInvalidTermNameStrings();
        }

        private static void InitInvalidTermNameStrings()
        {
            InvalidTermNameStrings = new List<string> { "&", ";", "<", ">", "|", "\"", "\t" };
        }

        #endregion

        #region properties

        public static List<string> InvalidTermNameStrings { get; set; }

        #endregion

        #region methods

        public static bool IsValidTermName(string value)
        {
            foreach (var invalidString in InvalidTermNameStrings)
                if (value.Contains(invalidString))
                    return false;

            return true;
        }

        #endregion
    }
}
