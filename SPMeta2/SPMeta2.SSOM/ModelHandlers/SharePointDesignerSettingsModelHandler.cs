using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Enumerations;
using System.Collections;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SharePointDesignerSettingsModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SharePointDesignerSettingsDefinition); }
        }

        protected virtual void SetPropertySafe(Hashtable properties, string key, object value)
        {
            if (!properties.ContainsKey(key))
                properties.Add(key, value);
            else
                properties[key] = value;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SharePointDesignerSettingsDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = site,
                ObjectType = typeof(SPSite),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var shouldUpdate = false;

            if (definition.EnableCustomizingMasterPagesAndPageLayouts.HasValue)
            {
                SetPropertySafe(rootWeb.AllProperties,
                               BuiltInWebPropertyId.AllowMasterpageEditing,
                               definition.EnableCustomizingMasterPagesAndPageLayouts.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableDetachingPages.HasValue)
            {
                SetPropertySafe(rootWeb.AllProperties,
                                BuiltInWebPropertyId.AllowRevertFromTemplate,
                                definition.EnableDetachingPages.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableManagingWebSiteUrlStructure.HasValue)
            {
                SetPropertySafe(rootWeb.AllProperties,
                                BuiltInWebPropertyId.ShowUrlStructure,
                                definition.EnableManagingWebSiteUrlStructure.Value == true ? 1 : 0);

                shouldUpdate = true;
            }

            if (definition.EnableSharePointDesigner.HasValue)
            {
                SetPropertySafe(rootWeb.AllProperties,
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
                ObjectType = typeof(SPSite),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (shouldUpdate)
            {
                rootWeb.Update();
            }
        }

        #endregion
    }
}
