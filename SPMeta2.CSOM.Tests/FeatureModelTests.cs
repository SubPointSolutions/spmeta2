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
    public class FeatureModelTests : ClientOMSharePointTestBase
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
        public void CanDeploySiteFeatures()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithSiteFeatures(siteFeatures =>
               {
                   siteFeatures
                       .AddFeature(FeatureSiteModels.Workflows);
               });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployWebFeatures()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
               .WithSiteFeatures(siteFeatures =>
               {
                   siteFeatures
                       .AddFeature(FeatureWebModels.WorkflowsCanUseAppPermissions);
               });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });


        }

        #endregion
    }
}
