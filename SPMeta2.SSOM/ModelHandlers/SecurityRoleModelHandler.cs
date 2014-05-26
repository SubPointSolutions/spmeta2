using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityRoleModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityRoleDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var web = modelHost.WithAssertAndCast<SPWeb>("modelHost", value => value.RequireNotNull());
            var securityRoleModel = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());

            var currentRoleDefinition = (SPRoleDefinition)null;

            var permissions = SPBasePermissions.EmptyMask;

            foreach (var permissionString in securityRoleModel.BasePermissions)
                permissions = permissions | (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), permissionString);

            try
            {
                currentRoleDefinition = web.RoleDefinitions[securityRoleModel.Name];
            }
            catch (SPException)
            {
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

            currentRoleDefinition.Update();
        }

        #endregion
    }
}
