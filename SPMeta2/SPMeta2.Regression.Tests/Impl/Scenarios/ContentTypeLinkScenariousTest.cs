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
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(f =>
                {
                    f.Enable = true;
                }));

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(f =>
                    {
                        f.Enable = true;
                    }));

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
        [TestCategory("Regression.Scenarios.ContentTypeLinks.CTH")]
        public void CanDeploy_ContentTypeLink_As_CTH_Item_ContentType()
        {
            if (!TestOptions.EnableContentTypeHubTests)
                return;

            WithDisabledPropertyUpdateValidation(() =>
            {
                var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                {
                    def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                    def.ContentTypesEnabled = true;
                });

                var ctLinkDef1 = new ContentTypeLinkDefinition
                {
                    ContentTypeName = "cth-item-1",
                    ContentTypeId = "0x01000FF176352927C44BB2DB4FBF2F30E88F"
                };

                var ctLinkDef2 = new ContentTypeLinkDefinition
                {
                    ContentTypeName = "cth-item-2",
                    ContentTypeId = "0x010072398EFE7B102948B9BEE545225CA462"
                };

                var webModel = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddList(listDef, list =>
                    {
                        list.AddContentTypeLink(ctLinkDef1);
                        //list.AddContentTypeLink(ctLinkDef2);
                    });
                });

                TestModel(webModel);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeLinks.CTH")]
        public void CanDeploy_ContentTypeLink_As_CTH_Document_ContentType()
        {
            if (!TestOptions.EnableContentTypeHubTests)
                return;

            WithDisabledPropertyUpdateValidation(() =>
            {
                var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                {
                    def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    def.ContentTypesEnabled = true;
                });

                var ctLinkDef1 = new ContentTypeLinkDefinition
                {
                    ContentTypeName = "cth-doc-1",
                    ContentTypeId = "0x01010021C1A5D40722E14591426E165F107547"
                };

                var ctLinkDef2 = new ContentTypeLinkDefinition
                {
                    ContentTypeName = "cth-doc-2",
                    ContentTypeId = "0x010100429E2FB078A6984385E2531F073EA963"
                };

                var webModel = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddList(listDef, list =>
                    {
                        list.AddContentTypeLink(ctLinkDef1);
                        //list.AddContentTypeLink(ctLinkDef2);
                    });
                });

                TestModel(webModel);
            });
        }

        #endregion

        #region content type hub

        #endregion
    }
}
