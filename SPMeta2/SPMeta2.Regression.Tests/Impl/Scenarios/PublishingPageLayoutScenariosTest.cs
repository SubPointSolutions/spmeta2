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
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Tests.Prototypes;
using SPMeta2.Regression.Tests.Utils;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class PublishingPageLayoutScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region publishing pages

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout")]
        public void CanDeploy_Default_PublishingPageLayout_AsSiteModel()
        {
            var models = WithPublishingPagelayoutNodeAsSite(null);

            TestModels(models);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout")]
        public void CanDeploy_Default_PublishingPageLayout_AsWebModel()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var pageLayoutContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.Page;
                def.Group = "Page Layout Content Types";
            });
            var pageLayout = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.AssociatedContentTypeId = pageLayoutContentType.GetContentTypeId();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(siteFeature);
                site.AddContentType(pageLayoutContentType);
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

        private IEnumerable<ModelNode> WithPublishingPagelayoutNodeAsSite(Action<ModelNode> pageSetup)
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var pageLayoutContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.Page;
                def.Group = "Page Layout Content Types";
            });
            var pageLayout = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.AssociatedContentTypeId = pageLayoutContentType.GetContentTypeId();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(siteFeature);
                site.AddContentType(pageLayoutContentType);

                // TODO

                //site.AddRootWeb(new RootWebDefinition(), rootWeb =>
                //{
                //    rootWeb.AddWebFeature(webFeature);
                //    rootWeb.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                //    {
                //        list.AddPublishingPageLayout(pageLayout, p =>
                //        {
                //            if (pageSetup != null)
                //                pageSetup(p);
                //        });
                //    });
                //});
            });

            return new[] { siteModel };
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout")]
        public void CanDeploy_Unpublished_PublishingPageLayout()
        {
            WithPublishingPagelayoutNodeAsSite(page =>
            {
                page.OnProvisioned<object>(context =>
                {
                    UnpublishFile(context);
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout")]
        public void CanDeploy_CheckOuted_PublishingPageLayout()
        {
            WithPublishingPagelayoutNodeAsSite(page =>
            {
                page.OnProvisioned<object>(context =>
                {
                    CheckoutFile(context);
                });
            });
        }

        #endregion

        #region field values



        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout.Values")]
        public void CanDeploy_Default_PublishingPageLayout_With_RequiredFieldValues()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var requiredText = RItemValues.GetRequiredTextField(ModelGeneratorService);

            var text1 = RItemValues.GetRandomTextField(ModelGeneratorService);
            var text2 = RItemValues.GetRandomTextField(ModelGeneratorService);

            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.PageLayout;
            });

            var publishingPageLayoutContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = string.Format("Required - {0}", Environment.TickCount);
                def.Hidden = false;
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.PageLayout;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.ContentTypeName = contentTypeDef.Name;
                def.AssociatedContentTypeId = publishingPageLayoutContentType.GetContentTypeId();

                def.DefaultValues.Add(new FieldValue()
                {
                    FieldName = requiredText.InternalName,
                    Value = Rnd.String()
                });


                def.Values.Add(new FieldValue()
                {
                    FieldName = text1.InternalName,
                    Value = Rnd.String()
                });

                def.Values.Add(new FieldValue()
                {
                    FieldName = text2.InternalName,
                    Value = Rnd.String()
                });
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddFeature(siteFeature);

                site.AddField(requiredText);
                site.AddField(text1);
                site.AddField(text2);

                site.AddContentType(contentTypeDef, contentType =>
                {
                    contentType.AddContentTypeFieldLink(requiredText);
                    contentType.AddContentTypeFieldLink(text1);
                    contentType.AddContentTypeFieldLink(text2);
                });
                site.AddContentType(publishingPageLayoutContentType);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddFeature(webFeature);

                web.AddList(BuiltInListDefinitions.Catalogs.MasterPage.Inherit(d =>
                {
                    d.ContentTypesEnabled = true;
                }), list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddContentTypeLink(publishingPageLayoutContentType);

                    list.AddPublishingPageLayout(itemDef);
                });
            });

            TestModels(new ModelNode[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PublishingPageLayout.Values")]
        public void CanDeploy_Default_PublishingPageLayout_With_ContentType_ByName()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.PageLayout;
            });

            var publishingPageLayoutContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = string.Format("Required - {0}", Environment.TickCount);
                def.Hidden = false;
                def.ParentContentTypeId = BuiltInPublishingContentTypeId.PageLayout;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<PublishingPageLayoutDefinition>(def =>
            {
                def.ContentTypeName = contentTypeDef.Name;
                def.AssociatedContentTypeId = publishingPageLayoutContentType.GetContentTypeId();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddFeature(siteFeature);

                site.AddContentType(contentTypeDef);
                site.AddContentType(publishingPageLayoutContentType);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddFeature(webFeature);

                web.AddList(BuiltInListDefinitions.Catalogs.MasterPage.Inherit(d =>
                {
                    d.ContentTypesEnabled = true;
                }), list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddPublishingPageLayout(itemDef);
                });
            });

            TestModel(siteModel, webModel);
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
