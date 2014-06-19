using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SecurityGroupLinkModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            if (modelHost is SecurableObject)
            {
                var securityGroupLinkModel = model as SecurityGroupLinkDefinition;
                if (securityGroupLinkModel == null) throw new ArgumentException("model has to be SecurityGroupDefinition");

                var web = GetWebFromSPSecurableObject(modelHost as SecurableObject);

                var context = web.Context;

                context.Load(web, w => w.SiteGroups);
                context.ExecuteQuery();

                var securityGroup = WebExtensions.FindGroupByName(web.SiteGroups, securityGroupLinkModel.SecurityGroupName);

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurableObject = modelHost as SecurableObject,
                    SecurityGroup = securityGroup
                };

                action(newModelHost);
            }
            else
            {
                action(modelHost);
            }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var securableObject = modelHost.WithAssertAndCast<SecurableObject>("modelHost", value => value.RequireNotNull());
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);
            var context = web.Context;

            context.Load(web, w => w.SiteGroups);
            context.Load(web, w => w.RoleDefinitions);

            context.Load(securableObject, s => s.HasUniqueRoleAssignments);
            context.Load(securableObject, s => s.RoleAssignments.Include(r => r.Member));

            context.ExecuteQuery();

            var securityGroup = WebExtensions.FindGroupByName(web.SiteGroups, securityGroupLinkModel.SecurityGroupName);

            if (!securableObject.HasUniqueRoleAssignments)
                securableObject.BreakRoleInheritance(false, false);

            var roleAssignment = FindRoleRoleAssignment(securableObject.RoleAssignments, securityGroup);

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = roleAssignment,
                ObjectType = typeof(RoleAssignment),
                ObjectDefinition = securityGroupLinkModel,
                ModelHost = modelHost
            });

            if (roleAssignment == null)
            {
                var bindings = new RoleDefinitionBindingCollection(context);
                bindings.Add(web.RoleDefinitions.GetByType(RoleType.Reader));

                var assegnment = securableObject.RoleAssignments.Add(securityGroup, bindings);

                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = assegnment,
                    ObjectType = typeof(RoleAssignment),
                    ObjectDefinition = securityGroupLinkModel,
                    ModelHost = modelHost
                });

                // GOTCHA!!! supposed to continue chain with adding role definitions via RoleDefinitionLinks
                bindings.RemoveAll();
            }
            else
            {
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = roleAssignment,
                    ObjectType = typeof(RoleAssignment),
                    ObjectDefinition = securityGroupLinkModel,
                    ModelHost = modelHost
                });
            }
        }

        protected RoleAssignment FindRoleRoleAssignment(RoleAssignmentCollection roleAssignments, Group securityGroup)
        {
            foreach (var ra in roleAssignments)
                if (ra.Member.Id == securityGroup.Id)
                    return ra;

            return null;
        }

        protected Web GetWebFromSPSecurableObject(SecurableObject securableObject)
        {
            if (securableObject is Web)
                return securableObject as Web;

            if (securableObject is List)
                return (securableObject as List).ParentWeb;

            if (securableObject is ListItem)
                return (securableObject as ListItem).ParentList.ParentWeb;

            throw new Exception(string.Format("Can't extract SPWeb for securableObject of type: [{0}]", securableObject.GetType()));
        }

        #endregion

        public override Type TargetType
        {
            get { return typeof(SecurityGroupLinkDefinition); }
        }
    }
}
