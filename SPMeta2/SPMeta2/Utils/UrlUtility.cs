using System;
using System.Collections.Generic;
using System.Linq;

namespace SPMeta2.Utils
{
    public static class UrlUtility
    {
        #region methods

        public static string RemoveStartingSlash(string url)
        {
            var result = url;

            while (result.StartsWith("/"))
                result = result.Remove(0, 1);

            if (result.StartsWith(@"\"))
                result = result.Remove(0, 1);

            return result;
        }

        public static string CombineUrl(IEnumerable<string> urls)
        {
            var items = urls.ToList();

            if (items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return items[0];

            var result = string.Empty;

            for (var i = 0; i < items.Count; i++)
                result = CombineUrl(result, items[i]);

            return result;
        }

        public static string CombineUrl(string baseUrlPath, string additionalNodes)
        {
            if (string.IsNullOrEmpty(baseUrlPath))
                return additionalNodes;

            if (string.IsNullOrEmpty(additionalNodes))
                return baseUrlPath;

            if (baseUrlPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                if (additionalNodes.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                    baseUrlPath = baseUrlPath.TrimEnd(new char[] { '/' });

                return baseUrlPath + additionalNodes;
            }

            if (additionalNodes.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                return baseUrlPath + additionalNodes;

            return baseUrlPath + "/" + additionalNodes;
        }

        #endregion
    }
}
