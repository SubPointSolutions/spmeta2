using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.SSOM;
using SPMeta2.Regression.Impl.Tests.Impl.Services.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.Services;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services.SSOM
{

    [TestClass]
    public class SSOMTokenReplacementServiceTests : TokenReplacementServiceTestBase
    {
        public SSOMTokenReplacementServiceTests()
        {

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

        protected override TokenReplacementServiceBase Service
        {
            get
            {
                if (base.Service == null)
                    base.Service = new SSOMTokenReplacementService();

                return base.Service;
            }
            set
            {
                base.Service = value;
            }
        }

        public SSOMProvisionRunner _SSOMProvisionRunner { get; set; }

        public SSOMProvisionRunner ProvisionRunner
        {
            get
            {
                if (_SSOMProvisionRunner == null)
                    _SSOMProvisionRunner = new SSOMProvisionRunner();

                return _SSOMProvisionRunner;
            }

            set
            {
                _SSOMProvisionRunner = value;
            }
        }

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

        [TestMethod]
        [TestCategory("Regression.Impl.SSOMTokenReplacementService")]
        public void SSOMTokenReplacementService_Should_Not_ChangeNonTokenedUrl()
        {
            var isValid = true;
            var runner = new SSOMProvisionRunner();

            runner.SiteUrls.ForEach(siteUrl =>
            {
                runner.WithSSOMSiteAndWebContext(siteUrl, (site, web) =>
                {
                    var originalFullUrl = site.Url;
                    var originalSiteRelativeUrl = site.ServerRelativeUrl;

                    var originalValueResult = Service.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = originalFullUrl,
                        Context = site
                    });

                    var originalSiteRelativeResult = Service.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = originalSiteRelativeUrl,
                        Context = site
                    });

                    Assert.AreEqual(originalFullUrl, originalValueResult.Value);
                    Assert.AreEqual(originalSiteRelativeUrl, originalSiteRelativeResult.Value);
                });
            });

            Assert.IsTrue(isValid);
        }

        #endregion
    }
}
