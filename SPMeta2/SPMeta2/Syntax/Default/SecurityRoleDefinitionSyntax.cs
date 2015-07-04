using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class SecurityRoleLinkModelNode : TypedModelNode
    {

    }

    public static class SecurityRoleLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSecurityRole<TModelNode>(this TModelNode model, SecurityRoleDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSecurityRole(model, definition, null);
        }

        public static TModelNode AddSecurityRole<TModelNode>(this TModelNode model, SecurityRoleDefinition definition,
            Action<ModuleFileModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSecurityRoles<TModelNode>(this TModelNode model, IEnumerable<SecurityRoleDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
