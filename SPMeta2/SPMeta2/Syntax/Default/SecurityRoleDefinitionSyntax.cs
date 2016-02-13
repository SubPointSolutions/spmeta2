using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SecurityRoleModelNode : TypedModelNode
    {

    }

    public static class SecurityRoleDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSecurityRole<TModelNode>(this TModelNode model, SecurityRoleDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSecurityRole(model, definition, null);
        }

        public static TModelNode AddSecurityRole<TModelNode>(this TModelNode model, SecurityRoleDefinition definition,
            Action<SecurityRoleModelNode> action)
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
