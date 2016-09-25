using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Services;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.CSOM;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Regression.Impl.Tests.Impl.Services.Base;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{
    [TestClass]
    public class CSOMTokenReplacementServiceTests : TokenReplacementServiceTestBase
    {
        #region constructors

        public CSOMTokenReplacementServiceTests()
        {
            Service = new CSOMTokenReplacementService();
            ProvisionRunner = new CSOMProvisionRunner();
        }

        #endregion

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

        public CSOMProvisionRunner ProvisionRunner { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        [TestCategory("CI.Core")]
        public void SelfDiagnostic_TestShouldHaveAllTokens()
        {
            var isValid = HasAllTestsForTokens("CSOM", Service.SupportedTokens, GetType().GetMethods());
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        public void CSOMTokenReplacementService_Can_Replace_SiteCollection_Token()
        {
            if (!CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext)
                return;

            var isValid = true;

            ProvisionRunner.SiteUrls.ForEach(siteUrl =>
            {
                ProvisionRunner.WithCSOMContext(siteUrl, context =>
                {
                    PreloadProperties(context);

                    var site = context.Site;

                    if (site.ServerRelativeUrl == "/")
                    {
                        isValid &= CheckRootSiteOnHost(context);
                    }
                    else
                    {
                        isValid &= CheckSubSiteOnHost(context, site.ServerRelativeUrl);
                    }
                });
            });

            Assert.IsTrue(isValid);
        }


        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        public void CSOMTokenReplacementService_Can_Replace_Site_Token()
        {
            if (!CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext)
                return;

            var isValid = true;
            var runner = new CSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithCSOMContext(siteUrl, context =>
                {
                    PreloadProperties(context);

                    var web = context.Web;

                    if (web.ServerRelativeUrl == "/")
                    {
                        isValid &= CheckRootWebOnHost(context);
                    }
                    else
                    {
                        isValid &= CheckSubWebOnHost(context, web.ServerRelativeUrl);
                    }
                });
            });

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        public void CSOMTokenReplacementService_Should_Support_ClientContext()
        {
            if (!CSOMTokenReplacementService.AllowClientContextAsTokenReplacementContext)
                return;

            var runner = new CSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithCSOMContext(siteUrl, context =>
                {
                    PreloadProperties(context);

                    var valueResult = Service.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = "~site",
                        Context = context
                    });
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        public void CSOMTokenReplacementService_Should_Support_WebModelHost()
        {
            var runner = new CSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithCSOMContext(siteUrl, context =>
                {
                    PreloadProperties(context);

                    var valueResult = Service.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = "~site",
                        Context = new WebModelHost
                        {
                            HostClientContext = context,

                            HostSite = context.Site,
                            HostWeb = context.Web
                        }
                    });
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOMTokenReplacementService")]
        public void CSOMTokenReplacementService_Should_Support_SiteModelHost()
        {
            var runner = new CSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithCSOMContext(siteUrl, context =>
                {
                    PreloadProperties(context);

                    var valueResult = Service.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = "~site",
                        Context = new SiteModelHost
                        {
                            HostClientContext = context,

                            HostSite = context.Site,
                            HostWeb = context.Web
                        }
                    });
                });
            });
        }

        #endregion

        #region utils

        private static void PreloadProperties(ClientContext clientContext)
        {
            var needQuery = false;

            if (!clientContext.Site.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Site, s => s.ServerRelativeUrl);
                needQuery = true;
            }

            if (!clientContext.Web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl);
                needQuery = true;
            }

            if (needQuery)
            {
                clientContext.ExecuteQueryWithTrace();
            }
        }

        #endregion
    }
}
