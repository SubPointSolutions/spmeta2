using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Exceptions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class PublishingPageScenarios : SPMeta2RegresionScenarioTestBase
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

        #region specific content type name

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_Default_PublishingPage_WithSpecificContentType()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var pageContentType1 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = string.Format("Publishing Page 1 - {0}", Rnd.String(8));
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.ArticlePage;
            });

            var pageContentType2 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = string.Format("Publishing Page 2 - {0}", Rnd.String(8));
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.ArticlePage;
            });

            var page1 = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>(def =>
            {
                def.ContentTypeName = pageContentType1.Name;
            });

            var page2 = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>(def =>
            {
                def.ContentTypeName = pageContentType2.Name;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(siteFeature);

                site.AddContentType(pageContentType1);
                site.AddContentType(pageContentType2);

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddContentTypeLink(pageContentType1);
                    list.AddContentTypeLink(pageContentType2);

                    list.AddPublishingPage(page1);
                    list.AddPublishingPage(page2);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        #endregion

        #region publishing pages

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_Default_PublishingPage()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var page = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddPublishingPage(page);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_Default_PublishingPage_WithRequiredFields()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var publishingPageLayoutContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = string.Format("Required - {0}", Environment.TickCount);
                def.Hidden = false;
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.ArticlePage;
            });

            var requiredText = ModelGeneratorService.GetRandomDefinition<TextFieldDefinition>(def =>
            {
                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInNewForm = true;
                def.ShowInVersionHistory = true;
                def.ShowInViewForms = true;

                def.ValidationFormula = null;
                def.ValidationMessage = null;

                def.Hidden = false;

                def.DefaultValue = string.Empty;
                def.Required = true;
            });

            var publishingPageLayout = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.AssociatedContentTypeId = publishingPageLayoutContentType.GetContentTypeId();
            });

            var page = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>(def =>
            {
                def.PageLayoutFileName = publishingPageLayout.FileName;

                def.DefaultValues.Add(new FieldValue()
                {
                    FieldName = requiredText.InternalName,
                    Value = Rnd.String()
                });
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(siteFeature);

                site.AddField(requiredText);

                site.AddContentType(publishingPageLayoutContentType, contentType =>
                {
                    contentType.AddContentTypeFieldLink(requiredText);
                });
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);

                web.AddHostList(BuiltInListDefinitions.Calalogs.MasterPage, list =>
                {
                    list.AddPublishingPageLayout(publishingPageLayout);
                });

                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddContentTypeLink(publishingPageLayoutContentType);

                    list.AddPublishingPage(page);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_Default_PublishingPageToFolder()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var page = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddPublishingPage(page);
                    });
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        private void WithPublishingPageNode(Action<ModelNode> pageSetup)
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var publishingPage = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddPublishingPage(publishingPage, page =>
                    {
                        pageSetup(page);
                    });
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_Unpublished_PublishingPage()
        {
            WithPublishingPageNode(page =>
            {
                page.OnProvisioned<object>(context =>
                {
                    UnpublishFile(context);
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPage")]
        public void CanDeploy_CheckOuted_PublishingPage()
        {
            WithPublishingPageNode(page =>
            {
                page.OnProvisioned<object>(context =>
                {
                    CheckoutFile(context);
                });
            });
        }

        #endregion

        #region utils

        private void CheckoutFile(OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.File"))
            {
                obj.CallMethod("CheckOut");
                var spContext = obj.GetPropertyValue("Context");

                spContext.CallMethod("ExecuteQuery");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPFile"))
            {
                obj.CallMethod(null, m => m.Name == "CheckOut" && m.GetParameters().Count() == 0);
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("CheckoutFile() method is not implemented for type: [{0}]", objType));
            }
        }

        private void UnpublishFile(OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.File"))
            {
                obj.CallMethod("UnPublish", new string[] { "unpublish" });
                var spContext = obj.GetPropertyValue("Context");

                spContext.CallMethod("ExecuteQuery");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPFile"))
            {
                obj.CallMethod("UnPublish", new string[] { "unpublish" });
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("UnpublishFile() method is not implemented for type: [{0}]", objType));
            }
        }

        #endregion
    }
}
