using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Utils
{
    public static class IndexedPropertyUtils
    {
        #region methods

        /// <summary>
        /// Method to create property bag for search index properties
        /// http://blogs.msdn.com/b/vesku/archive/2013/10/12/ftc-to-cam-setting-indexed-property-bag-keys-using-csom.aspx
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetEncodedValueForSearchIndexProperty(IEnumerable<string> keys)
        {
            var stringBuilder = new StringBuilder();

            foreach (var current in keys)
            {
                stringBuilder.Append(Convert.ToBase64String(Encoding.Unicode.GetBytes(current)));
                stringBuilder.Append('|');
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Decode the IndexedPropertyKeys value so it's readable
        /// https://lixuan0125.wordpress.com/2014/07/24/make-property-bags-searchable-in-sharepoint-2013/
        /// </summary>
        /// <param name="encodedValue"></param>
        /// <returns></returns>
        public static List<string> GetDecodeValueForSearchIndexProperty(string encodedValue)
        {
            if (string.IsNullOrEmpty(encodedValue))
                return new List<string>();

            var keys = encodedValue.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            return keys.Select(current => Encoding.Unicode.GetString(Convert.FromBase64String(current))).ToList();
        }

        #endregion
    }
}
