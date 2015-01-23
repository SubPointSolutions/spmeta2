using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityRoleLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityRoleLinkDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
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
            else if (securableObject is SPSecurableObject)
            {
                ProcessSPSecurableObjectHost(securableObject as SPSecurableObject, securityGroup, securityRoleLinkModel);
            }
            else
            {
                throw new Exception(string.Format("modelHost of type:[{0}] is not supported.", modelHost.GetType()));
            }
        }

        protected SPWeb ExtractWeb(object modelHost)
        {
            if (modelHost is SPWeb)
                return modelHost as SPWeb;

            if (modelHost is SPList)
                return (modelHost as SPList).ParentWeb;

            if (modelHost is SPListItem)
                return (modelHost as SPListItem).ParentList.ParentWeb;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.ParentWeb;

            if (modelHost is FolderModelHost)
                return (modelHost as FolderModelHost).CurrentLibraryFolder.ParentWeb;

            throw new Exception(string.Format("modelHost with type [{0}] is not supported.", modelHost.GetType()));
        }

        protected SPRoleDefinition ResolveSecurityRole(SPWeb web, SecurityRoleLinkDefinition rolDefinitionModel)
        {
            var roleDefinitions = web.RoleDefinitions;

            if (!string.IsNullOrEmpty(rolDefinitionModel.SecurityRoleName))
            {
                foreach (SPRoleDefinition roleDefinition in roleDefinitions)
                    if (string.Compare(roleDefinition.Name, rolDefinitionModel.SecurityRoleName, true) == 0)
                        return roleDefinition;
            }
            else if (rolDefinitionModel.SecurityRoleId > 0)
            {
                foreach (SPRoleDefinition roleDefinition in roleDefinitions)
                {
                    if (roleDefinition.Id == rolDefinitionModel.SecurityRoleId)
                        return roleDefinition;
                }
            }
            else if (!string.IsNullOrEmpty(rolDefinitionModel.SecurityRoleType))
            {
                var roleType = (SPRoleType)Enum.Parse(typeof(SPRoleType), rolDefinitionModel.SecurityRoleType, true);

                return roleDefinitions.GetByType(roleType);
            }

            throw new ArgumentException(string.Format("Cannot resolve role definition for role definition link model:[{0}]", rolDefinitionModel));
        }

        private void ProcessSPSecurableObjectHost(SPSecurableObject targetSecurableObject, SPGroup securityGroup,
            SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            //// TODO
            // need common validation infrastructure 
            var web = ExtractWeb(targetSecurableObject);

            var roleAssignment = new SPRoleAssignment(securityGroup);

            var role = ResolveSecurityRole(web, securityRoleLinkModel);

            if (!roleAssignment.RoleDefinitionBindings.Contains(role))
            {
                if (roleAssignment.RoleDefinitionBindings.Count == 1
                         && roleAssignment.RoleDefinitionBindings[0].Type == SPRoleType.Reader)
                {
                    roleAssignment.RoleDefinitionBindings.RemoveAll();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = targetSecurableObject
                });

                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new security role link");

                roleAssignment.RoleDefinitionBindings.Add(role);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing security role link");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = role,
                    ObjectType = typeof(SPRoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = targetSecurableObject
                });

            }

            targetSecurableObject.RoleAssignments.Add(roleAssignment);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = role,
                ObjectType = typeof(SPRoleDefinition),
                ObjectDefinition = securityRoleLinkModel,
                ModelHost = targetSecurableObject
            });
        }

        private void ProcessSPGroupHost(SPGroup modelHost, SPGroup securityGroup, SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            // TODO
            // need common validation infrastructure 
            var web = securityGroup.ParentWeb;

            var securityRoleAssignment = new SPRoleAssignment(securityGroup);
            SPRoleDefinition roleDefinition = ResolveSecurityRole(web, securityRoleLinkModel);

            if (!securityRoleAssignment.RoleDefinitionBindings.Contains(roleDefinition))
            {
                if (securityRoleAssignment.RoleDefinitionBindings.Count == 1
                         && securityRoleAssignment.RoleDefinitionBindings[0].Type == SPRoleType.Reader)
                {
                    securityRoleAssignment.RoleDefinitionBindings.RemoveAll();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
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
                InvokeOnModelEvent(this, new ModelEventArgs
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

            InvokeOnModelEvent(this, new ModelEventArgs
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
