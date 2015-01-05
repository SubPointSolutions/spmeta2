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

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TreeViewSettingsModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(TreeViewSettingsDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TreeViewSettingsDefinition>("model", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (definition.QuickLaunchEnabled.HasValue)
                web.QuickLaunchEnabled = definition.QuickLaunchEnabled.Value;

            if (definition.TreeViewEnabled.HasValue)
                web.TreeViewEnabled = definition.TreeViewEnabled.Value;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (definition.QuickLaunchEnabled.HasValue || definition.TreeViewEnabled.HasValue)
                web.Update();
        }

        #endregion
    }
}
