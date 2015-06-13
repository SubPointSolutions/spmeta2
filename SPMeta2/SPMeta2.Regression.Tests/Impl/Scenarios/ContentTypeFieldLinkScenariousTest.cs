using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ContentTypeFieldLinkScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region by id or by internla name

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink.IdOrName")]
        public void CanDeploy_ContentTypeFieldLink_WithFieldId()
        {
            var siteFieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var siteFieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(siteFieldOne);
                site.AddField(siteFieldTwo);

                site.AddContentType(siteContentType, c =>
                {
                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldId = siteFieldOne.Id
                    });

                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldId = siteFieldTwo.Id
                    });
                });
            });

            TestModels(new[] { siteModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink.IdOrName")]
        public void CanDeploy_ContentTypeFieldLink_WithFieldInternalName()
        {
            var siteFieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var siteFieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(siteFieldOne);
                site.AddField(siteFieldTwo);

                site.AddContentType(siteContentType, c =>
                {
                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldInternalName = siteFieldOne.InternalName
                    });

                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldInternalName = siteFieldTwo.InternalName
                    });
                });
            });

            TestModels(new[] { siteModel });
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink.Scopes")]
        public void CanDeploy_ContentTypeFieldLink_OnWebContentType()
        {
            var siteFieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var siteFieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var webFieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var webFieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var webContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(siteFieldOne);
                site.AddField(siteFieldTwo);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddField(webFieldOne);
                    subWeb.AddField(webFieldTwo);

                    subWeb.AddContentType(webContentType, contentType =>
                    {
                        contentType
                            .AddContentTypeFieldLink(siteFieldOne)
                            .AddContentTypeFieldLink(siteFieldTwo)
                            .AddContentTypeFieldLink(webFieldOne)
                            .AddContentTypeFieldLink(webFieldTwo);
                    });
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink.Scopes")]
        public void CanDeploy_ContentTypeFieldLink_OnSiteContentType()
        {
            var siteFieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var siteFieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(siteFieldOne);
                site.AddField(siteFieldTwo);

                site.AddContentType(siteContentType, c =>
                {
                    c.AddContentTypeFieldLink(siteFieldOne);
                    c.AddContentTypeFieldLink(siteFieldTwo);
                });
            });

            TestModels(new[] { siteModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink")]
        public void CanDeploy_ContentTypeFieldLink_WithProperties()
        {
            var fieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var fieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var contentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(fieldOne);
                site.AddField(fieldTwo);

                site.AddContentType(contentType, c =>
                {
                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldId = fieldOne.Id,
                        DisplayName = Rnd.String(),
                        Hidden = Rnd.Bool(),
                        Required = Rnd.Bool()
                    });

                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        DisplayName = Rnd.String(),
                        FieldId = fieldTwo.Id,
                        Hidden = Rnd.Bool(),
                        Required = Rnd.Bool()
                    });
                });
            });

            TestModels(new[] { siteModel });
        }

        #endregion
    }
}
