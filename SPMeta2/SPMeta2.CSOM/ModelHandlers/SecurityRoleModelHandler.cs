using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SecurityRoleModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SecurityRoleDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var securityRoleModel = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            var roleDefinitions = web.RoleDefinitions;

            context.Load(roleDefinitions);
            context.ExecuteQuery();

            var currentRoleDefinition = FindRoleDefinition(roleDefinitions, securityRoleModel.Name);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentRoleDefinition,
                ObjectType = typeof(RoleDefinition),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            var basePermissions = new BasePermissions();

            foreach (var permissionString in securityRoleModel.BasePermissions)
                basePermissions.Set((PermissionKind)Enum.Parse(typeof(PermissionKind), permissionString));

            if (currentRoleDefinition == null)
            {
                currentRoleDefinition = roleDefinitions.Add(new RoleDefinitionCreationInformation
                {
                    Name = securityRoleModel.Name,
                    BasePermissions = basePermissions,
                    Description = securityRoleModel.Description ?? string.Empty
                });
            }

            // SPBug, 
            // something wrong with setting up BasePermissions.Set() method up 
            // so, a new object has to be assigned every time
            currentRoleDefinition.BasePermissions = basePermissions;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentRoleDefinition,
                ObjectType = typeof(RoleDefinition),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            currentRoleDefinition.Update();

            context.ExecuteQuery();
        }

        protected RoleDefinition FindRoleDefinition(RoleDefinitionCollection roleDefinitions, string roleDefinitionName)
        {
            foreach (var roleDefinition in roleDefinitions)
                if (string.Compare(roleDefinition.Name, roleDefinitionName, true) == 0)
                    return roleDefinition;

            return null;
        }

        private Group FindSecurityGroupByTitle(IEnumerable<Group> siteGroups, string securityGroupTitle)
        {
            // gosh, who cares ab GetById() methods?! Where GetByName()?!

            foreach (var securityGroup in siteGroups)
            {
                if (System.String.Compare(securityGroup.Title, securityGroupTitle, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return securityGroup;
            }

            return null;
        }

        #endregion
    }
}
