using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SharePointDesignerSettingsModelHandler : CSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SharePointDesignerSettingsDefinition); }
        }

        protected virtual void SetPropertySafe(PropertyValues properties, string key, object value)
        {
            properties[key] = value.ToString();
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SharePointDesignerSettingsDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            var context = site.Context;

            context.Load(rootWeb, w => w.AllProperties);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = site,
                ObjectType = typeof(Site),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var shouldUpdate = false;
            var properties = rootWeb.AllProperties;

            if (definition.EnableCustomizingMasterPagesAndPageLayouts.HasValue)
            {
                SetPropertySafe(properties,
                               BuiltInWebPropertyId.AllowMasterpageEditing,
                               definition.EnableCustomizingMasterPagesAndPageLayouts.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableDetachingPages.HasValue)
            {
                SetPropertySafe(properties,
                                BuiltInWebPropertyId.AllowRevertFromTemplate,
                                definition.EnableDetachingPages.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableManagingWebSiteUrlStructure.HasValue)
            {
                SetPropertySafe(properties,
                                BuiltInWebPropertyId.ShowUrlStructure,
                                definition.EnableManagingWebSiteUrlStructure.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableSharePointDesigner.HasValue)
            {
                SetPropertySafe(properties,
                               BuiltInWebPropertyId.AllowDesigner,
                               definition.EnableSharePointDesigner.Value == true ? 1 : 0);

                shouldUpdate = true;
            }


            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = site,
                ObjectType = typeof(Site),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (shouldUpdate)
            {
                siteModelHost.ShouldUpdateHost = false;

                rootWeb.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        #endregion
    }
}
