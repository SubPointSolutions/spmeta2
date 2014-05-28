using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    [TestClass]
    public class WebPartPagesModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanCreateWebPartPages()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                  .WithLists(lists =>
                                  {
                                      lists
                                          .AddList(ListModels.SitePages, list =>
                                          {
                                              list
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page1)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page2)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page3)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page4)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page5)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page6)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page7)
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page8);
                                          });

                                  });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });

        }

        #endregion
    }
}
