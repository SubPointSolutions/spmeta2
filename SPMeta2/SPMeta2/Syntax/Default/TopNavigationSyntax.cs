using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class TopNavigationNodeModelNode : TypedModelNode
    {

    }

    public static class TopNavigationSyntax
    {
        public static WebModelNode AddTopNavigationNode(this WebModelNode model, TopNavigationNodeDefinition definition)
        {
            return AddTopNavigationNode(model, definition, null);
        }

        public static WebModelNode AddTopNavigationNode(this WebModelNode model, TopNavigationNodeDefinition definition,
            Action<TopNavigationNodeModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static TopNavigationNodeModelNode AddTopNavigationNode(this TopNavigationNodeModelNode model, TopNavigationNodeDefinition definition)
        {
            return AddTopNavigationNode(model, definition, null);
        }

        public static TopNavigationNodeModelNode AddTopNavigationNode(this TopNavigationNodeModelNode model, TopNavigationNodeDefinition definition,
            Action<TopNavigationNodeModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }
    }
}
