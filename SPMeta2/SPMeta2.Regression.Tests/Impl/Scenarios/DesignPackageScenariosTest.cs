using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;
using System.IO;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class DesignPackageScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        public DesignPackageScenariosTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
        }

        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.DesignPackage")]
        public void CanDeploy_Simple_DesignPackage()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var designPackageDef = ModelGeneratorService.GetRandomDefinition<DesignPackageDefinition>(def =>
            {

            });

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var rootWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
            });

            var designPackageModel = SPMeta2Model.NewSiteModel(site => site.AddDesignPackage(designPackageDef));

            TestModels(new ModelNode[]
            {
                siteModel, 
                rootWebModel, 
                designPackageModel
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DesignPackage")]
        public void CanDeploy_Simple_DesignPackage_As_Apply()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var designPackageDef = ModelGeneratorService.GetRandomDefinition<DesignPackageDefinition>(def =>
            {
                def.Apply = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var rootWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
            });

            var designPackageModel = SPMeta2Model.NewSiteModel(site => site.AddDesignPackage(designPackageDef));

            TestModels(new ModelNode[]
            {
                siteModel, 
                rootWebModel, 
                designPackageModel
            });
        }


        #endregion
    }
}
