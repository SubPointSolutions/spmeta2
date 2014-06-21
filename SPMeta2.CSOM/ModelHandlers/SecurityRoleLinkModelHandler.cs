using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class SecurityRoleLinkModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SecurityRoleLinkDefinition); }
        }

        #endregion

        #region methods

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

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var securityGroupModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var securityRoleLinkModel = model.WithAssertAndCast<SecurityRoleLinkDefinition>("model", value => value.RequireNotNull());

            var securableObject = securityGroupModelHost.SecurableObject;

            var group = securityGroupModelHost.SecurityGroup;
            var web = GetWebFromSPSecurableObject(securityGroupModelHost.SecurableObject);

            var context = group.Context;
            var existingRoleAssignments = web.RoleAssignments;

            context.Load(existingRoleAssignments, r => r.Include(d => d.Member, d => d.RoleDefinitionBindings));
            context.ExecuteQuery();

            RoleAssignment existingRoleAssignment = null;

            foreach (var roleAs in existingRoleAssignments)
            {
                if (roleAs.Member.Id == group.Id)
                {
                    existingRoleAssignment = roleAs;
                    break;
                }
            }


            var roleDefinitions = web.RoleDefinitions;

            context.Load(roleDefinitions);
            context.ExecuteQuery();

            var currentRoleDefinition = FindRoleDefinition(roleDefinitions, securityRoleLinkModel.SecurityRoleName);


            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentRoleDefinition,
                ObjectType = typeof(RoleDefinition),
                ObjectDefinition = securityRoleLinkModel,
                ModelHost = modelHost
            });

            // MESSY, refactor

            if (existingRoleAssignment == null)
            {
                var roleBindings = new RoleDefinitionBindingCollection(context);
                roleBindings.Add(currentRoleDefinition);
                existingRoleAssignment = web.RoleAssignments.Add(group, roleBindings);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentRoleDefinition,
                    ObjectType = typeof(RoleDefinition),
                    ObjectDefinition = securityRoleLinkModel,
                    ModelHost = modelHost
                });

                existingRoleAssignment.Update();
                context.ExecuteQuery();
            }
            else
            {
                var ensureDefinition = true;

                foreach (var t in existingRoleAssignment.RoleDefinitionBindings)
                {
                    if (t.Name == currentRoleDefinition.Name)
                    {
                        ensureDefinition = false;
                        break;
                    }
                }

                if (ensureDefinition)
                {
                    existingRoleAssignment.RoleDefinitionBindings.Add(currentRoleDefinition);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentRoleDefinition,
                        ObjectType = typeof(RoleDefinition),
                        ObjectDefinition = securityRoleLinkModel,
                        ModelHost = modelHost
                    });

                    existingRoleAssignment.Update();
                    context.ExecuteQuery();
                }
                else
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentRoleDefinition,
                        ObjectType = typeof(RoleDefinition),
                        ObjectDefinition = securityRoleLinkModel,
                        ModelHost = modelHost
                    });
                }
            }
        }

        private RoleDefinition FindRoleDefinition(RoleDefinitionCollection roleDefinitions, string roleDefinitionName)
        {
            foreach (var roleDefinition in roleDefinitions)
                if (string.Compare(roleDefinition.Name, roleDefinitionName, true) == 0)
                    return roleDefinition;

            return null;
        }

        #endregion
    }
}
