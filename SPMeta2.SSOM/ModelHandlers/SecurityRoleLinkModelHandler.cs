using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityRoleLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityRoleLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var securityRoleLinkModel = model.WithAssertAndCast<SecurityRoleLinkDefinition>("model", value => value.RequireNotNull());

            var securableObject = modelHostContext.SecurableObject;
            var securityGroup = modelHostContext.SecurityGroup;

            if (securableObject == null || securableObject is SPWeb)
            {
                // this is SPGroup -> SPRoleLink deployment
                ProcessSPGroupHost(securityGroup, securityGroup, securityRoleLinkModel);
            }
            else if (securableObject is SPList)
            {
                ProcessSPListHost(securableObject as SPList, securityGroup, securityRoleLinkModel);
            }
            else
            {
                throw new Exception(string.Format("modelHost of type:[{0}] is not supported.", modelHost.GetType()));
            }
        }

        private void ProcessSPListHost(SPList targetList, SPGroup securityGroup, 
            SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            //// TODO
            // need common validation infrastructure 
            var web = targetList.ParentWeb;

            var roleAssignment = new SPRoleAssignment(securityGroup);

            var role = web.RoleDefinitions[securityRoleLinkModel.SecurityRoleName];

            if (!roleAssignment.RoleDefinitionBindings.Contains(role))
            {
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = targetList
                });

                roleAssignment.RoleDefinitionBindings.Add(role);
            }
            else
            {
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = role,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = targetList
                });

            }

            targetList.RoleAssignments.Add(roleAssignment);

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = role,
                ObjectType = typeof(SPRoleDefinition),
                ObjectDefinition = securityRoleLinkModel,
                ModelHost = targetList
            });

            targetList.Update();
        }

        private void ProcessSPGroupHost(SPGroup modelHost, SPGroup securityGroup, SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            // TODO
            // need common validation infrastructure 
            var web = securityGroup.ParentWeb;

            var securityRoleAssignment = new SPRoleAssignment(securityGroup);
            var roleDefinition = web.RoleDefinitions[securityRoleLinkModel.SecurityRoleName];

            if (!securityRoleAssignment.RoleDefinitionBindings.Contains(roleDefinition))
            {
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = modelHost
                });

                securityRoleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            }
            else
            {
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = roleDefinition,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = modelHost
                });
            }

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = roleDefinition,
                ObjectType = typeof(SPRoleDefinition),
                ObjectDefinition = securityRoleLinkModel,
                ModelHost = modelHost
            });

            web.RoleAssignments.Add(securityRoleAssignment);
            web.Update();
        }

        #endregion
    }
}
