using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityGroupLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityGroupLinkDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var securableObject = ExtractSecurableObject(modelHost);

            if (securableObject is SPSecurableObject)
            {
                var securityGroupLinkModel = model as SecurityGroupLinkDefinition;
                if (securityGroupLinkModel == null) throw new ArgumentException("model has to be SecurityGroupDefinition");

                var web = ExtractWeb(modelHost);
                var securityGroup = ResolveSecurityGroup(web, securityGroupLinkModel);

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurableObject = securableObject as SPSecurableObject,
                    SecurityGroup = securityGroup
                };

                action(newModelHost);
            }
            else
            {
                action(modelHost);
            }
        }

        private SPWeb ExtractWeb(object modelHost)
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

        protected SPGroup ResolveSecurityGroup(SPWeb web, SecurityGroupLinkDefinition securityGroupLinkModel)
        {
            SPGroup securityGroup = null;

            if (securityGroupLinkModel.IsAssociatedMemberGroup)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "IsAssociatedMemberGroup = true. Resolving AssociatedMemberGroup");
                securityGroup = web.AssociatedMemberGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedOwnerGroup)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "IsAssociatedOwnerGroup = true. Resolving IsAssociatedOwnerGroup");
                securityGroup = web.AssociatedOwnerGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedVisitorGroup)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "IsAssociatedVisitorGroup = true. Resolving IsAssociatedVisitorGroup");
                securityGroup = web.AssociatedVisitorGroup;
            }
            else if (!string.IsNullOrEmpty(securityGroupLinkModel.SecurityGroupName))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving group by name: [{0}]", securityGroupLinkModel.SecurityGroupName);
                securityGroup = web.SiteGroups[securityGroupLinkModel.SecurityGroupName];
            }
            else
            {
                TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "IsAssociatedMemberGroup/IsAssociatedOwnerGroup/IsAssociatedVisitorGroup/SecurityGroupName should be defined. Throwing SPMeta2Exception");

                throw new SPMeta2Exception("securityGroupLinkModel");
            }

            return securityGroup;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            if (!securableObject.HasUniqueRoleAssignments)
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "securableObject.HasUniqueRoleAssignments = false. Breaking with false-false options.");
                securableObject.BreakRoleInheritance(false, false);
            }

            var web = GetWebFromSPSecurableObject(securableObject);

            var securityGroup = ResolveSecurityGroup(web, securityGroupLinkModel);
            var roleAssignment = securableObject.RoleAssignments
                                                       .OfType<SPRoleAssignment>()
                                                       .FirstOrDefault(a => a.Member.ID == securityGroup.ID);

            var isNewRoleAssignment = false;

            if (roleAssignment == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject,
                    "Processing new security group link");

                roleAssignment = new SPRoleAssignment(securityGroup);
                isNewRoleAssignment = true;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing security group link");
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = roleAssignment,
                ObjectType = typeof(SPRoleAssignment),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (isNewRoleAssignment)
            {
                // add default guest role as hidden one
                // we need to at least one role in order to create assignment 
                // further provision will chech of there is only one role - Reader, and will remove it
                roleAssignment.RoleDefinitionBindings.Add(web.RoleDefinitions.GetByType(SPRoleType.Reader));
            }

            securableObject.RoleAssignments.Add(roleAssignment);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = roleAssignment,
                ObjectType = typeof(SPRoleAssignment),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
        }

        protected SPSecurableObject ExtractSecurableObject(object modelHost)
        {
            return SecurableHelper.ExtractSecurableObject(modelHost);
        }

        protected SPWeb GetWebFromSPSecurableObject(SPSecurableObject securableObject)
        {
            if (securableObject is SPWeb)
                return securableObject as SPWeb;

            if (securableObject is SPList)
                return (securableObject as SPList).ParentWeb;

            if (securableObject is SPListItem)
                return (securableObject as SPListItem).ParentList.ParentWeb;

            throw new Exception(string.Format("Can't extract SPWeb for securableObject of type: [{0}]", securableObject.GetType()));
        }

        protected virtual SPUser EnsureOwnerUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.Owner))
            {
                return web.Site.Owner;
            }
            else
            {
                return web.EnsureUser(groupModel.Owner);
            }
        }

        protected virtual SPUser EnsureDefaultUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.DefaultUser))
            {
                return null;
            }
            else
            {
                return web.EnsureUser(groupModel.DefaultUser);
            }
        }

        #endregion
    }

    internal static class SecurableHelper
    {
        public static SPSecurableObject ExtractSecurableObject(object modelHost)
        {
            if (modelHost is SPSecurableObject)
                return modelHost as SPSecurableObject;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            if (modelHost is FolderModelHost)
            {
                var folderHost = (modelHost as FolderModelHost);

                if (folderHost.CurrentLibraryFolder != null)
                    return folderHost.CurrentLibraryFolder.Item;
                else
                    return folderHost.CurrentListItem;
            }

            if (modelHost is WebpartPageModelHost)
                return (modelHost as WebpartPageModelHost).PageListItem;

            throw new SPMeta2NotImplementedException(string.Format("Model host of type:[{0}] is not supported by SecurityGroupLinkModelHandler yet.",
                modelHost.GetType()));
        }
    }
}
