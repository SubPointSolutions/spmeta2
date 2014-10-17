using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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
            var securableObject = ExtractSecurableObject(modelHost);

            if (securableObject is SecurableObject)
            {
                var securityGroupLinkModel = model as SecurityGroupLinkDefinition;
                if (securityGroupLinkModel == null) throw new ArgumentException("model has to be SecurityGroupDefinition");

                var web = GetWebFromSPSecurableObject(securableObject as SecurableObject);

                var context = web.Context;

                context.Load(web, w => w.SiteGroups);
                context.ExecuteQuery();

                Group securityGroup = ResolveSecurityGroup(securityGroupLinkModel, web, context);

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurableObject = securableObject,
                    SecurityGroup = securityGroup
                };

                action(newModelHost);
            }
            else
            {
                action(modelHost);
            }
        }

        protected SecurableObject ExtractSecurableObject(object modelHost)
        {
            return SecurableHelper.ExtractSecurableObject(modelHost);
        }


        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);
            var context = web.Context;

            context.Load(web, w => w.SiteGroups);
            context.Load(web, w => w.RoleDefinitions);

            context.Load(securableObject, s => s.HasUniqueRoleAssignments);
            context.Load(securableObject, s => s.RoleAssignments.Include(r => r.Member));

            context.ExecuteQuery();

            Group securityGroup = ResolveSecurityGroup(securityGroupLinkModel, web, context);

            if (!securableObject.HasUniqueRoleAssignments)
                securableObject.BreakRoleInheritance(false, false);

            var roleAssignment = FindRoleRoleAssignment(securableObject.RoleAssignments, securityGroup);

            InvokeOnModelEvent(this, new ModelEventArgs
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

                InvokeOnModelEvent(this, new ModelEventArgs
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
                InvokeOnModelEvent(this, new ModelEventArgs
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

        protected virtual Group ResolveSecurityGroup(SecurityGroupLinkDefinition securityGroupLinkModel, Web web, ClientRuntimeContext context)
        {
            Group securityGroup = null;

            if (securityGroupLinkModel.IsAssociatedMemberGroup)
            {
                context.Load(web, w => w.AssociatedMemberGroup);
                context.ExecuteQuery();

                securityGroup = web.AssociatedMemberGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedOwnerGroup)
            {
                context.Load(web, w => w.AssociatedOwnerGroup);
                context.ExecuteQuery();

                securityGroup = web.AssociatedOwnerGroup;
            }
            else if (securityGroupLinkModel.IsAssociatedVisitorGroup)
            {
                context.Load(web, w => w.AssociatedVisitorGroup);
                context.ExecuteQuery();

                securityGroup = web.AssociatedVisitorGroup;
            }
            else if (!string.IsNullOrEmpty(securityGroupLinkModel.SecurityGroupName))
            {
                securityGroup = WebExtensions.FindGroupByName(web.SiteGroups, securityGroupLinkModel.SecurityGroupName);
            }
            else
            {
                throw new ArgumentException("securityGroupLinkModel");
            }
            return securityGroup;
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
