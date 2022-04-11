using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class AssociatedGroupsModelNode : TypedModelNode,
        IWebModelNode
    {

    }

    public static class AssociatedGroupsDefinitionSyntax
    {

        #region methods

        public static TModelNode AddAssociatedGroups<TModelNode>(this TModelNode model, AssociatedGroupsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddAssociatedGroups(model, definition, null);
        }

        public static TModelNode AddAssociatedGroups<TModelNode>(this TModelNode model, AssociatedGroupsDefinition definition,
            Action<AssociatedGroupsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
