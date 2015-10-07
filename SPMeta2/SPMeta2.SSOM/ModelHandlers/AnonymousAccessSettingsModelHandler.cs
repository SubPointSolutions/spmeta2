using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class AnonymousAccessSettingsModelHandler : SSOMModelHandlerBase
    {

        public override Type TargetType
        {
            get { return typeof(AnonymousAccessSettingsDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<AnonymousAccessSettingsDefinition>("model", value => value.RequireNotNull());

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

            if (!web.HasUniqueRoleAssignments)
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall,
                    "web.HasUniqueRoleAssignments = false. Breaking with false-false options.");
                web.BreakRoleInheritance(false, false);
            }

            var anonState = (SPWeb.WebAnonymousState)Enum.Parse(typeof(SPWeb.WebAnonymousState), definition.AnonymousState);
            web.AnonymousState = anonState;

            if (definition.AnonymousPermMask64.Any())
            {
                var permissions = SPBasePermissions.EmptyMask;

                foreach (var permissionString in definition.AnonymousPermMask64)
                    permissions = permissions | (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), permissionString);

                web.AnonymousPermMask64 = permissions;
            }

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

            web.Update();
        }
    }
}
