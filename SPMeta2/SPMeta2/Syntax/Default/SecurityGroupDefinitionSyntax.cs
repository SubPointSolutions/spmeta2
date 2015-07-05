using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public class SecurityGroupModelNode : ListItemModelNode
    {

    }

    public static class SecurityGroupDefinitionSyntax
    {

        #region methods

        public static TModelNode AddSecurityGroup<TModelNode>(this TModelNode model, SecurityGroupDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSecurityGroup(model, definition, null);
        }

        public static TModelNode AddSecurityGroup<TModelNode>(this TModelNode model, SecurityGroupDefinition definition,
            Action<SecurityGroupModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSecurityGroups<TModelNode>(this TModelNode model, IEnumerable<SecurityGroupDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
