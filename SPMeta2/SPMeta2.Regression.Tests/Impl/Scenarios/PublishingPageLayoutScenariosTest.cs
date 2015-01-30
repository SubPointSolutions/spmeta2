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
using System.Threading.Tasks;
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
                rootWeb.AddHostList(BuiltInListDefinitions.Calalogs.MasterPage, list =>
                {
                    list.AddPublishingPageLayout(pageLayout);
                });
            });

            TestModels(new[] { siteModel, webModel });
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

                site.AddRootWeb(new RootWebDefinition(), rootWeb =>
                {
                    rootWeb.AddWebFeature(webFeature);
                    rootWeb.AddHostList(BuiltInListDefinitions.Calalogs.MasterPage, list =>
                    {
                        list.AddPublishingPageLayout(pageLayout, p =>
                        {
                            if (pageSetup != null)
                                pageSetup(p);
                        });
                    });
                });
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

    internal static class ReflectionHelpers
    {
        internal static object CallMethod(this object obj, string methodName)
        {
            return CallMethod(obj, methodName, null);
        }

        internal static object CallMethod(this object obj, string methodName, object[] parameters)
        {
            return CallMethod(obj, parameters, m => m.Name == methodName);
        }

        internal static object CallMethod(this object obj, object[] parameters,
            Func<MethodInfo, bool> filter)
        {
            var type = obj.GetType();
            var method = type.GetMethods().FirstOrDefault(m => filter(m) == true);

            return method.Invoke(obj, parameters);
        }

        internal static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
    }
}
