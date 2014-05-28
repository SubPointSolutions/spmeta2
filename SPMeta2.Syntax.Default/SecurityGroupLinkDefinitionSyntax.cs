using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityGroupLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupLinkDefinition securityGroupLinkDefinition)
        {
            return AddSecurityGroupLink(model, securityGroupLinkDefinition, null);
        }

        public static ModelNode AddSecurityGroupLink(this ModelNode model, SecurityGroupLinkDefinition securityGroupLinkDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = securityGroupLinkDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupDefinition securityGroupDefinition)
        {
            return AddSecurityGroupLink(model, securityGroupDefinition, null);
        }

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupDefinition securityGroupDefinition, Action<ModelNode> action)
        {
            return AddSecurityGroupLink(model, new SecurityGroupLinkDefinition
            {
                SecurityGroupName = securityGroupDefinition.Name
            }, action);
        }

        #endregion

        #region model

        //public static IEnumerable<SecurityGroupLinkDefinition> GetSecurityGroupLinks(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<SecurityGroupLinkDefinition>();
        //}

        #endregion
    }
}
