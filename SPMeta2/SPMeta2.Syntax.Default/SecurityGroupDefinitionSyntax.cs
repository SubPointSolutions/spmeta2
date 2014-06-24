using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityGroupDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityGroup(this ModelNode model, SecurityGroupDefinition definition)
        {
            return AddSecurityGroup(model, definition, null);
        }

        public static ModelNode AddSecurityGroup(this ModelNode model, SecurityGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
