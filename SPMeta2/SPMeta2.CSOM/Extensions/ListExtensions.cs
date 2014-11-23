using System;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Extensions
{
    public static class ListExtensions
    {
        #region methods

        public static ListItem QueryAndGetItemByFileName(this List list, string fileName)
        {
            var context = list.Context;
            var items = list.GetItems(CamlQueryTemplates.ItemByFileNameQuery(fileName));

            context.Load(items, i => i);
            context.ExecuteQueryWithTrace();

            if (items.Count < 1)
                throw new Exception(string.Format("Can't find ListItem by filename:[{0}]", fileName));

            return items[0];
        }

        #endregion
    }
}
