using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class NavigationNodeModelNode : TypedModelNode
    {
    }

    [Serializable]
    [DataContract]
    public class QuickLaunchNavigationNodeModelNode : NavigationNodeModelNode,
        IQuickLaunchNavigationNodeHostModelNode
    {

    }

    public static class QuickLaunchNavigationNodeDefinitionSyntax
    {
        #region methods

        public static TModelNode AddQuickLaunchNavigationNode<TModelNode>(this TModelNode model, QuickLaunchNavigationNodeDefinition definition)
            where TModelNode : ModelNode, IQuickLaunchNavigationNodeHostModelNode, new()
        {
            return AddQuickLaunchNavigationNode(model, definition, null);
        }

        public static TModelNode AddQuickLaunchNavigationNode<TModelNode>(this TModelNode model, QuickLaunchNavigationNodeDefinition definition,
            Action<QuickLaunchNavigationNodeModelNode> action)
            where TModelNode : ModelNode, IQuickLaunchNavigationNodeHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddQuickLaunchNavigationNodes<TModelNode>(this TModelNode model, IEnumerable<QuickLaunchNavigationNodeDefinition> definitions)
           where TModelNode : ModelNode, IQuickLaunchNavigationNodeHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
