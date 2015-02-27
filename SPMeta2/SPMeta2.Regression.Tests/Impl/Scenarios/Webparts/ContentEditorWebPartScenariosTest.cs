using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.CSOM;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Exceptions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Webparts
{
    [TestClass]
    public class ContentEditorWebPartScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region list binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentEditorWebPart.Tokens")]
        public void CanDeploy_ContentEditorWebPart_WithContentLink_With_Tokens()
        {
            var cewpSiteCollectionDefinition = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
            {
                def.ContentLink = "~sitecollection/style library/my content.html";
            });

            var cewpSiteDefinition = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
            {
                def.ContentLink = "~site/style library/my content.html";
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddContentEditorWebPart(cewpSiteCollectionDefinition);
                                    page.AddContentEditorWebPart(cewpSiteDefinition);
                                });
                        });
                });

            TestModel(model);
        }


        #endregion
    }
}
