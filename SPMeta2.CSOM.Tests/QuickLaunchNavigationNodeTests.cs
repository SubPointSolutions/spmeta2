using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class QuickLaunchNavigationNodeTests : ClientOMSharePointTestBase
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
        public void CanDeployRootQuickLaunchNavigation()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .WithQuickLaunchNavigation(nav =>
                {
                    nav
                        .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.RootHome)
                        .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.Help, help =>
                        {
                            help
                                .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.ItHelp)
                                .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.MedHelp);
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
