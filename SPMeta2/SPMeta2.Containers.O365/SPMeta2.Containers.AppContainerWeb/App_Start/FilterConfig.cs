using System.Web;
using System.Web.Mvc;

namespace SPMeta2.Containers.AppContainerWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
