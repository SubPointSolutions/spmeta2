using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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

        protected Web ExtractWeb(SecurableObject securableObject)
        {
            if (securableObject is Web)
                return securableObject as Web;

            if (securableObject is List)
                return (securableObject as List).ParentWeb;

            if (securableObject is ListItem)
                return (securableObject as ListItem).ParentList.ParentWeb;

            throw new Exception(string.Format("Can't extract SPWeb for securableObject of type: [{0}]", securableObject.GetType()));
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securityGroupModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var securityRoleLinkModel = model.WithAssertAndCast<SecurityRoleLinkDefinition>("model", value => value.RequireNotNull());

            var securableObject = securityGroupModelHost.SecurableObject;

            var group = securityGroupModelHost.SecurityGroup;
            var web = ExtractWeb(securityGroupModelHost.SecurableObject);

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



            var currentRoleDefinition = ResolveSecurityRole(web, securityRoleLinkModel);

            context.Load(currentRoleDefinition, r => r.Id, r => r.Name);
            context.ExecuteQuery();

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

        protected RoleDefinition ResolveSecurityRole(Web web, SecurityRoleLinkDefinition rolDefinitionModel)
        {
            var context = web.Context;
            var roleDefinitions = web.RoleDefinitions;

            context.Load(roleDefinitions, r => r.Include(l => l.Name, l => l.Id));
            context.ExecuteQuery();

            if (!string.IsNullOrEmpty(rolDefinitionModel.SecurityRoleName))
            {
                foreach (var roleDefinition in roleDefinitions)
                    if (string.Compare(roleDefinition.Name, rolDefinitionModel.SecurityRoleName, true) == 0)
                        return roleDefinition;
            }
            else if (rolDefinitionModel.SecurityRoleId > 0)
            {
                foreach (var roleDefinition in roleDefinitions)
                {
                    if (roleDefinition.Id == rolDefinitionModel.SecurityRoleId)
                        return roleDefinition;
                }
            }
            else if (!string.IsNullOrEmpty(rolDefinitionModel.SecurityRoleType))
            {
                var roleType = (RoleType)Enum.Parse(typeof(RoleType), rolDefinitionModel.SecurityRoleType, true);

                return roleDefinitions.GetByType(roleType);
            }

            throw new ArgumentException(string.Format("Cannot resolve role definition for role definition link model:[{0}]", rolDefinitionModel));
        }

        #endregion
    }
}
