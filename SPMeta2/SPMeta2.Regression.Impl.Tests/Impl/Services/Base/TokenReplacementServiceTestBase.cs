using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Services;
using SPMeta2.Regression.Utils;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services.Base
{
    public class TokenReplacementServiceTestBase
    {
        protected virtual TokenReplacementServiceBase Service { get; set; }

        protected bool HasAllTestsForTokens(string methodPrefix, IEnumerable<TokenInfo> tokens,
            IEnumerable<MethodInfo> methods)
        {
            var isValid = true;

            foreach (var token in tokens)
            {
                var tokenName = token.Name.Replace("~", string.Empty);

                var methodName = string.Format("{1}TokenReplacementService_Can_Replace_{0}_Token", tokenName,
                    methodPrefix);
                var method = methods.FirstOrDefault(m => m.Name.ToUpper() == methodName.ToUpper());

                RegressionUtils.WriteLine(methodName + " : " + (method != null));

                if (method == null)
                    isValid = false;
            }

            return isValid;
        }

        protected bool CheckSubSiteOnHost(object context, string siteServerRelativeUrl)
        {
            var isValid = true;

            // ~sitecollection -> siteUrl.ServerRelativeUrl on /sites/my-site-collection
            isValid &= ShouldPass("~sitecollection -> string.Empty", context, "~sitecollection", siteServerRelativeUrl);
            isValid &= ShouldPass("~SiteCollection -> string.Empty", context, "~SiteCollection", siteServerRelativeUrl);

            // ~sitecollection/something -> /something on the root web
            isValid &= ShouldPass("~sitecollection/something1 -> /something1", context, "~sitecollection/something1", siteServerRelativeUrl + "/something1");
            isValid &= ShouldPass("~SiteCollection/something2 -> /something2", context, "~SiteCollection/something2", siteServerRelativeUrl + "/something2");

            // same same
            isValid &= ShouldPass("sitecollection/something1 -> sitecollection/something1", context, "sitecollection/something1", "sitecollection/something1");
            isValid &= ShouldPass("SiteCollection/something2 -> SiteCollection/something2", context, "SiteCollection/something2", "SiteCollection/something2");

            return isValid;
        }

        protected bool ShouldPass(string message, object context, string token, string expectedUrl)
        {
            var result = true;

            RegressionUtils.WriteLine(message);

            var valueResult = Service.ReplaceTokens(new TokenReplacementContext
            {
                Value = token,
                Context = context
            });

            result = expectedUrl.ToUpper() == valueResult.Value.ToUpper();

            RegressionUtils.WriteLine(string.Format("[{0}] - [{1}] - token:[{2}] expected value:[{3}] replaced value:[{4}]",
                new object[]
                {
                    message,
                    result,
                    token,
                    expectedUrl,
                    valueResult.Value
                }));

            return result;
        }

        protected bool CheckRootSiteOnHost(object context)
        {
            var isValid = true;

            // ~sitecollection -> string.Empty on the root web
            isValid &= ShouldPass("~sitecollection -> string.Empty", context, "~sitecollection", "/");
            isValid &= ShouldPass("~SiteCollection -> string.Empty", context, "~SiteCollection", "/");

            // ~sitecollection/something -> /something on the root web
            isValid &= ShouldPass("~sitecollection/something1 -> /something1", context, "~sitecollection/something1", "/something1");
            isValid &= ShouldPass("~SiteCollection/something2 -> /something2", context, "~SiteCollection/something2", "/something2");

            // same same
            isValid &= ShouldPass("sitecollection/something1 -> sitecollection/something1", context, "sitecollection/something1", "sitecollection/something1");
            isValid &= ShouldPass("SiteCollection/something2 -> SiteCollection/something2", context, "SiteCollection/something2", "SiteCollection/something2");

            return isValid;
        }

        protected bool CheckSubWebOnHost(object context, string webServerRelativeUrl)
        {
            var isValid = true;

            // ~site -> siteUrl.ServerRelativeUrl on /sites/my-site-collection
            isValid &= ShouldPass("~site -> string.Empty", context, "~site", webServerRelativeUrl);
            isValid &= ShouldPass("~Site -> string.Empty", context, "~Site", webServerRelativeUrl);

            // ~site/something -> /something on the root web
            isValid &= ShouldPass("~site/something1 -> /something1", context, "~site/something1", webServerRelativeUrl + "/something1");
            isValid &= ShouldPass("~Site/something2 -> /something2", context, "~Site/something2", webServerRelativeUrl + "/something2");

            // same same
            isValid &= ShouldPass("site/something1 -> site/something1", context, "site/something1", "site/something1");
            isValid &= ShouldPass("Site/something2 -> Site/something2", context, "Site/something2", "Site/something2");

            return isValid;
        }

        protected bool CheckRootWebOnHost(object context)
        {
            var isValid = true;

            // ~site -> / on the root web
            isValid &= ShouldPass("~site -> string.Empty", context, "~site", "/");
            isValid &= ShouldPass("~Site -> string.Empty", context, "~Site", "/");

            // ~site/ -> / on the root web
            isValid &= ShouldPass("~site/ -> string.Empty", context, "~site/", "/");
            isValid &= ShouldPass("~Site/ -> string.Empty", context, "~Site/", "/");

            // ~site/something -> /something on the root web
            isValid &= ShouldPass("~site/something1 -> /something1", context, "~site/something1", "/something1");
            isValid &= ShouldPass("~Site/something2 -> /something2", context, "~Site/something2", "/something2");

            // ~site/something/ -> /something on the root web
            isValid &= ShouldPass("~site/something1/ -> /something1", context, "~site/something1/", "/something1");
            isValid &= ShouldPass("~Site/something2/ -> /something2", context, "~Site/something2/", "/something2");

            // same same
            isValid &= ShouldPass("site/something1 -> site/something1", context, "site/something1", "site/something1");
            isValid &= ShouldPass("Site/something2 -> site/something2", context, "Site/something2", "Site/something2");

            return isValid;
        }

    }
}
