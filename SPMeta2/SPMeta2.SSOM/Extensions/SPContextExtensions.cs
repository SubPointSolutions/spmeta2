using System;
using System.IO;
using System.Web;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.Extensions
{
    public class SPContextExtensions
    {
        /// <summary>
        /// Util method to be able to deplot CQWP from c#/ps code outside of SPContext.Curent == null context
        /// More details here:
        /// http://solutionizing.net/2009/02/16/faking-spcontext/
        /// http://blog.mastykarz.nl/structured-and-repeatable-deployment-of-content-query-web-part-instances/
        /// </summary>
        /// <param name="contextWeb"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void WithFakeSPContextScope(SPWeb contextWeb, Action action)
        {
            bool cleanHttpContext = false;

            try
            {
                if (HttpContext.Current == null)
                {
                    HttpRequest request = new HttpRequest(string.Empty, contextWeb.Url, string.Empty);
                    HttpContext.Current = new HttpContext(request, new HttpResponse(TextWriter.Null));

                    cleanHttpContext = true;
                }

                if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                    HttpContext.Current.Items["HttpHandlerSPWeb"] = contextWeb;

                action();
            }
            finally
            {
                // that's one of the most important part
                // if you don't clean up, some unpredoctable thing might happen w/ c#/PS and so on
                if (cleanHttpContext)
                {
                    try
                    {
                        HttpContext.Current = null;
                    }
                    catch { }
                }
            }
        }
    }
}
