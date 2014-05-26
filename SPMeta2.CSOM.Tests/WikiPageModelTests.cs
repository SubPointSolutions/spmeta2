using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    [TestClass]
    public class WikiPageModelTests : ClientOMSharePointTestBase
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
        public void CanDeployWikiPages()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                 .WithLists(lists =>
                                 {
                                     lists
                                         .AddList(ListModels.SitePages, list =>
                                         {
                                             list
                                                 .WithWikiPages(pages =>
                                                 {
                                                     pages
                                                         .AddWikiPage(WikiPageModels.SitePages.ProducstPage)
                                                         .AddWikiPage(WikiPageModels.SitePages.TermsPage);
                                                 });

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
