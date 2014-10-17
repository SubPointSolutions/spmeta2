using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
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
                securityGroup = web.AssociatedMemberGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedOwnerGroup)
            {
                securityGroup = web.AssociatedOwnerGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedVisitorGroup)
            {
                securityGroup = web.AssociatedVisitorGroup;
            }
            else if (!string.IsNullOrEmpty(securityGroupLinkModel.SecurityGroupName))
            {
                securityGroup =web.SiteGroups[securityGroupLinkModel.SecurityGroupName];
            }
            else
            {
                throw new ArgumentException("securityGroupLinkModel");
            }
            return securityGroup;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            if (!securableObject.HasUniqueRoleAssignments)
            {
                throw new SPMeta2Exception("securableObject does not have HasUniqueRoleAssignments. Please use BreakRoleInheritanceDefinition object or break role inheritable manually before deploying SecurityGroupLinkDefinition.");
            }

            var web = GetWebFromSPSecurableObject(securableObject);

            var securityGroup = ResolveSecurityGroup(web, securityGroupLinkModel);
            var roleAssignment = securableObject.RoleAssignments
                                                       .OfType<SPRoleAssignment>()
                                                       .FirstOrDefault(a => a.Member.ID == securityGroup.ID);

            var isNewRoleAssignment = false;

            if (roleAssignment == null)
            {
                roleAssignment = new SPRoleAssignment(securityGroup);
                isNewRoleAssignment = true;
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
                // default one, it will be removed later
                // we need at least one role for a new role assignment created
                // it will be deleted later

                var dummyRole = web.RoleDefinitions.GetByType(SPRoleType.Reader);

                if (!roleAssignment.RoleDefinitionBindings.Contains(dummyRole))
                    roleAssignment.RoleDefinitionBindings.Add(dummyRole);
            }

            securableObject.RoleAssignments.Add(roleAssignment);

            if (isNewRoleAssignment)
            {
                // removing dummy role for a new assignment created
                var tmpAssignment = securableObject.RoleAssignments
                                                       .OfType<SPRoleAssignment>()
                                                       .FirstOrDefault(a => a.Member.ID == securityGroup.ID);

                while (tmpAssignment.RoleDefinitionBindings.Count > 0)
                    tmpAssignment.RoleDefinitionBindings.Remove(0);

                tmpAssignment.Update();
            }

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
            if (modelHost is SPSecurableObject)
                return modelHost as SPSecurableObject;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            if (modelHost is FolderModelHost)
                return (modelHost as FolderModelHost).CurrentLibraryFolder.Item;

            if (modelHost is WebpartPageModelHost)
                return (modelHost as WebpartPageModelHost).PageListItem;

            throw new SPMeta2NotImplementedException(string.Format("Model host of type:[{0}] is not supported by SecurityGroupLinkModelHandler yet.",
                modelHost.GetType()));
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
}
