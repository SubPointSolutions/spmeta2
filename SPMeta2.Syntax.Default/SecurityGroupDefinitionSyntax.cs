using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityGroupDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityGroup(this ModelNode model, SecurityGroupDefinition securityGroup)
        {
            return AddSecurityGroup(model, securityGroup, null);
        }

        public static ModelNode AddSecurityGroup(this ModelNode model, SecurityGroupDefinition securityGroup, Action<ModelNode> securityGroupAction)
        {
            var securityGroupModelNode = new ModelNode { Value = securityGroup };

            model.ChildModels.Add(securityGroupModelNode);

            if (securityGroupAction != null)
                securityGroupAction(securityGroupModelNode);

            return model;
        }

        #endregion
    }
}
