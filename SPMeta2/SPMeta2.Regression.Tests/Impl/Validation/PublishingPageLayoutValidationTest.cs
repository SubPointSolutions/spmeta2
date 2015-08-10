using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Validation
{
    [TestClass]
    public class PublishingPageLayoutValidationTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region group link options

        [TestMethod]
        [TestCategory("Regression.Validation.PublishingPageLayout")]
        public void Validation_PublishingPageLayout_ShouldAllow_EmptyTitleAndDescription()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var pageLayout = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.Title = string.Empty;
                def.Description = string.Empty;

                def.AssociatedContentTypeId = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(siteFeature);
            });

            var webModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWebFeature(webFeature);

                rootWeb.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                {
                    list.AddPublishingPageLayout(pageLayout);
                });
            });

            TestModels(new ModelNode[] { siteModel, webModel });

        }


        #endregion
    }
}
