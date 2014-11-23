using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Extensions
{
    public static class WebExtensions
    {
        #region methods

        public static Group FindGroupByName(GroupCollection groups, string groupName)
        {
            foreach (var group in groups)
                if (group.Title == groupName)
                    return group;

            return null;
        }

        public static List QueryAndGetListByTitle(this Web web, string listTitle)
        {
            var context = web.Context;
            var list = web.Lists.GetByTitle(listTitle);

            context.Load(web, w => w.Lists);
            context.ExecuteQueryWithTrace();

            return list;
        }

        public static List QueryAndGetListByUrl(this Web web, string listUrl)
        {
            var context = web.Context;
            var lists = web.Lists;

            context.Load(lists, lc => lc.Include(
                    l => l.RootFolder,
                    l => l.Id,
                    l => l.Title
                ));

            context.ExecuteQueryWithTrace();

            List result = null;

            foreach (var list in lists)
            {
                // woow...
                if (list.RootFolder.ServerRelativeUrl.ToLower().EndsWith(listUrl.ToLower()))
                {
                    result = list;
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}
