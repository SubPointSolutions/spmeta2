using System;
using System.Linq;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.Extensions
{
    public static class SPViewCollectionExtensions
    {
        #region methods

        /// <summary>
        /// SPBug, "name" and "Name" are different (sic!)
        /// </summary>
        /// <param name="views"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static SPView FindByName(this SPViewCollection views, string viewName)
        {
            return views
                    .OfType<SPView>()
                    .FirstOrDefault(v => String.Compare(v.Title, viewName, StringComparison.OrdinalIgnoreCase) == 0);

        }

        #endregion
    }
}
