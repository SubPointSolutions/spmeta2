using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    //[TestClass]
    public class WebModelTests : ClientOMSharePointTestBase
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
        public void CanDeployWebs()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .WithSubWebs(webs =>
                {
                    webs
                        .AddWeb(WebModels.Projects, projectSite =>
                        {
                            projectSite
                                .AddWeb(WebModels.P1, p1 =>
                                {
                                    p1
                                        .AddWeb(WebModels.P1)
                                        .AddWeb(WebModels.P2);

                                })
                                .AddWeb(WebModels.P2);
                        })
                        .AddWeb(WebModels.Teams);
                });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        #endregion
    }
}
