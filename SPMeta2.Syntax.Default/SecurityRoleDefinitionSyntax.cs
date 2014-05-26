using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityRoleLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityRole(this ModelNode model, SecurityRoleDefinition securityRoleDefinition)
        {
            model.ChildModels.Add(new ModelNode { Value = securityRoleDefinition });

            return model;
        }

        #endregion

        #region model

        //public static IEnumerable<SecurityRoleDefinition> GetSecurityRoles(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<SecurityRoleDefinition>();
        //}

        #endregion
    }
}
