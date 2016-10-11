using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SharePointDesignerSettingsModelHandler : CSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SharePointDesignerSettingsDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SharePointDesignerSettingsDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            var context = site.Context;

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

            if (definition.EnableCustomizingMasterPagesAndPageLayouts.HasValue)
            {
                // TODO

                shouldUpdate = true;
            }

            if (definition.EnableDetachingPages.HasValue)
            {
                // TODO

                shouldUpdate = true;
            }

            if (definition.EnableManagingWebSiteUrlStructure.HasValue)
            {
                // TODO

                shouldUpdate = true;
            }

            if (definition.EnableSharePointDesigner.HasValue)
            {
                // TODO

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
                // TODO
                context.ExecuteQueryWithTrace();
            }
        }

        #endregion
    }
}
