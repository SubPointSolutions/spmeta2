using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ContentTypeLinkScenariousTest : SPMeta2RegresionScenarioTestBase
    {
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
        [TestCategory("Regression.Scenarios.ContentTypeLinks.Scopes")]
        public void CanDeploy_ContentTypeLink_With_SiteAndWebContentTypes()
        {
            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();
            var webContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var webList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(siteContentType);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddContentType(webContentType);

                    subWeb.AddList(webList, list =>
                    {
                        list.AddContentTypeLink(siteContentType);
                        list.AddContentTypeLink(webContentType);
                    });
                });
            });

            TestModel(siteModel, webModel);
        }

        #endregion

        #region read-only and sealed content types

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeLinks.Scopes")]
        public void CanDeploy_ContentTypeLink_With_SealedAndReadOnly_ContentTypes()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
                {
                    def.ReadOnly = true;
                    def.Sealed = true;
                });

                var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                {
                    def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                    def.ContentTypesEnabled = true;
                });

                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddContentType(contentTypeDef);
                });

                var webModel = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddList(listDef, list =>
                    {
                        list.AddContentTypeLink(contentTypeDef);
                    });
                });

                TestModel(siteModel, webModel);
            });
        }

        #endregion
    }
}
