using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
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
            TestModel(webModel);
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
            var type = obj.GetType();
            var method = type.GetMethod(methodName);

            return method.Invoke(obj, parameters);
        }

        internal static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
    }
}
