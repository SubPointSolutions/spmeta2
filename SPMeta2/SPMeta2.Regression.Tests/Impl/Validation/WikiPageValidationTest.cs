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
    public class WikiPageValidationTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Validation.WikiPage")]
        public void Validation_WikiPage_ShouldAllow_EmptyTitle()
        {
            var webFeature = BuiltInWebFeatures.WikiPageHomePage.Inherit(f => f.Enable());

            var wikiPage = ModelGeneratorService.GetRandomDefinition<WikiPageDefinition>(def =>
            {
                def.Title = string.Empty;
            });

            var webModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWebFeature(webFeature);

                rootWeb.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddWikiPage(wikiPage);
                });
            });

            TestModels(new ModelNode[] { webModel });
        }


        #endregion
    }
}
