using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityRoleDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityRoleLink(this  ModelNode model, SecurityRoleDefinition securityRoleDefinition)
        {
            return AddSecurityRoleLink(model, securityRoleDefinition, null);
        }

        public static ModelNode AddSecurityRoleLink(this ModelNode model, SecurityRoleDefinition securityRoleDefinition, Action<ModelNode> action)
        {
            var roleLinkDefinition = new SecurityRoleLinkDefinition
            {
                SecurityRoleName = securityRoleDefinition.Name
            };

            var newModelNode = new ModelNode { Value = roleLinkDefinition };

            model.ChildModels.Add(newModelNode);
            if (action != null) action(newModelNode);

            return model;
        }

        public static ModelNode AddSecurityRoleLink(this  ModelNode model, string securityRoleName)
        {
            var newSecurityRoleLink = new SecurityRoleLinkDefinition
            {
                SecurityRoleName = securityRoleName
            };

            var newModelNode = new ModelNode { Value = newSecurityRoleLink };

            model.ChildModels.Add(newModelNode);

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<SecurityRoleLinkDefinition> GetSecurityRoleLinks(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<SecurityRoleLinkDefinition>();
        //}

        #endregion
    }
}
