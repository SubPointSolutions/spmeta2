using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

// legacy
namespace SPMeta2.Syntax.Default.Extensions
{
    internal class Tmp1
    {
        
    }
}

// new
namespace SPMeta2.Syntax.Default
{
    public static class TypedModelNodeExtensions
    {
        #region methods

        public static TParentModeNode AddTypedDefinitionNode<TParentModeNode, TModeNode>(this TParentModeNode node, DefinitionBase definition)
            where TModeNode : ModelNode, new()
            where TParentModeNode : ModelNode, new()
        {
            return AddTypedDefinitionNode<TParentModeNode, TModeNode>(node, definition, null);
        }

        public static TParentModeNode AddTypedDefinitionNode<TParentModeNode, TModeNode>(this TParentModeNode node, DefinitionBase definition, Action<TModeNode> action)
            where TModeNode : ModelNode, new()
            where TParentModeNode : ModelNode, new()
        {
            return AddTypedDefinitionNodeWithOptions(node, definition, action, null);
        }

        public static TParentModeNode AddTypedDefinitionNodeWithOptions<TParentModeNode, TModeNode>(this TParentModeNode node,
            DefinitionBase definition,
            Action<TModeNode> action, ModelNodeOptions options)
            where TModeNode : ModelNode, new()
            where TParentModeNode : ModelNode, new()
        {
            var modelNode = new TModeNode { Value = definition };

            // TODO, should be extend later
            if (options != null)
            {
                modelNode.Options = options;

                //definition.RequireSelfProcessing = options.RequireSelfProcessing;
            }

            node.ChildModels.Add(modelNode);

            if (action != null)
                action(modelNode);

            return node;
        }

        #endregion

    }
}
