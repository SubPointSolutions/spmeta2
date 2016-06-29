using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Services;
using System.Reflection;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.CSOM;
using SPMeta2.Containers.Services;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Containers.SSOM;
using SPMeta2.Regression.Impl.Tests.Impl.Services.Base;
using SPMeta2.SSOM.Services;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{

    [TestClass]
    public class SSOMTokenReplacementServiceTests : TokenReplacementServiceTestBase
    {
        public SSOMTokenReplacementServiceTests()
        {
            Service = new SSOMTokenReplacementService();
            ProvisionRunner = new SSOMProvisionRunner();
        }

        #region init

        [ClassInitialize]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanup]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties

        public SSOMProvisionRunner ProvisionRunner { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.SSOMTokenReplacementService")]
        [TestCategory("CI.Core")]
        public void SelfDiagnostic_TestShouldHaveAllTokens()
        {
            var isValid = HasAllTestsForTokens("SSOM", Service.SupportedTokens, GetType().GetMethods());
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.SSOMTokenReplacementService")]
        public void SSOMTokenReplacementService_Can_Replace_SiteCollection_Token()
        {
            var isValid = true;

            ProvisionRunner.SiteUrls.ForEach(siteUrl =>
            {
                ProvisionRunner.WithSSOMSiteAndWebContext(siteUrl, (site, web) =>
                {
                    if (site.ServerRelativeUrl == "/")
                    {
                        isValid &= CheckRootSiteOnHost(site);
                        isValid &= CheckRootSiteOnHost(web);
                        isValid &= CheckRootSiteOnHost(web.Lists[0]);
                    }
                    else
                    {
                        isValid &= CheckSubSiteOnHost(site, site.ServerRelativeUrl);
                        isValid &= CheckSubSiteOnHost(web, site.ServerRelativeUrl);
                        isValid &= CheckSubSiteOnHost(web.Lists[0], site.ServerRelativeUrl);
                    }
                });
            });

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.SSOMTokenReplacementService")]
        public void SSOMTokenReplacementService_Can_Replace_Site_Token()
        {
            var isValid = true;
            var runner = new SSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithSSOMSiteAndWebContext(siteUrl, (site, web) =>
                {
                    if (site.ServerRelativeUrl == "/")
                    {
                        isValid &= CheckRootWebOnHost(site);
                        isValid &= CheckRootWebOnHost(web);
                        isValid &= CheckRootWebOnHost(web.Lists[0]);
                    }
                    else
                    {
                        isValid &= CheckSubWebOnHost(site, web.ServerRelativeUrl);
                        isValid &= CheckSubWebOnHost(web, web.ServerRelativeUrl);
                        isValid &= CheckSubWebOnHost(web.Lists[0], web.ServerRelativeUrl);
                    }
                });
            });

            Assert.IsTrue(isValid);
        }

        #endregion
    }
}
