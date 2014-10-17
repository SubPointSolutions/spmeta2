using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationPages.Calendar.Exchange;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityRoleModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityRoleDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var web = site.RootWeb;

            var securityRoleModel = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());
            var currentRoleDefinition = (SPRoleDefinition)null;

            var permissions = SPBasePermissions.EmptyMask;

            foreach (var permissionString in securityRoleModel.BasePermissions)
                permissions = permissions | (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), permissionString);

            try
            {
                currentRoleDefinition = web.RoleDefinitions[securityRoleModel.Name];

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentRoleDefinition,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleModel,
                    ModelHost = modelHost
                });
            }
            catch (SPException)
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleModel,
                    ModelHost = modelHost
                });

                web.RoleDefinitions.Add(new SPRoleDefinition
                {
                    Name = securityRoleModel.Name,
                    Description = securityRoleModel.Description,
                    BasePermissions = permissions
                });

                currentRoleDefinition = web.RoleDefinitions[securityRoleModel.Name];
            }

            currentRoleDefinition.Description = securityRoleModel.Description;
            currentRoleDefinition.BasePermissions = permissions;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentRoleDefinition,
                ObjectType = typeof(SPRoleDefinition),
                ObjectDefinition = securityRoleModel,
                ModelHost = modelHost
            });

            currentRoleDefinition.Update();
        }

        #endregion
    }
}
