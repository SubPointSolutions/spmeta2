using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class TopNavigationNodeModelNode : NavigationNodeModelNode,
        ITopNavigationNodeHostModelNode
    {

    }

    public static class TopNavigationSyntax
    {
        #region methods

        public static TModelNode AddTopNavigationNode<TModelNode>(this TModelNode model, TopNavigationNodeDefinition definition)
            where TModelNode : ModelNode, ITopNavigationNodeHostModelNode, new()
        {
            return AddTopNavigationNode(model, definition, null);
        }

        public static TModelNode AddTopNavigationNode<TModelNode>(this TModelNode model, TopNavigationNodeDefinition definition,
            Action<TopNavigationNodeModelNode> action)
            where TModelNode : ModelNode, ITopNavigationNodeHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTopNavigationNodes<TModelNode>(this TModelNode model, IEnumerable<TopNavigationNodeDefinition> definitions)
           where TModelNode : ModelNode, ITopNavigationNodeHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
