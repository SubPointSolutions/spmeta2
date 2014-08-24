using System;
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
                var securityGroup = web.SiteGroups[securityGroupLinkModel.SecurityGroupName];

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurableObject = modelHost as SPSecurableObject,
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

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).CurrentList.ParentWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            throw new Exception(string.Format("modelHost with type [{0}] is not supported.", modelHost.GetType()));
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);

            var securityGroup = web.SiteGroups[securityGroupLinkModel.SecurityGroupName];
            var roleAssignment = new SPRoleAssignment(securityGroup);

            // default one, it will be removed later
            var dummyRole = web.RoleDefinitions.GetByType(SPRoleType.Reader);



            if (!roleAssignment.RoleDefinitionBindings.Contains(dummyRole))
                roleAssignment.RoleDefinitionBindings.Add(dummyRole);

            // this is has to be decided later - what is the strategy fro breaking role inheritance
            if (!securableObject.HasUniqueRoleAssignments)
                securableObject.BreakRoleInheritance(false);

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

            // GOTCHA!!! supposed to continue chain with adding role definitions via RoleDefinitionLinks
            roleAssignment.RoleDefinitionBindings.RemoveAll();
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
                return (modelHost as ListModelHost).CurrentList;

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
