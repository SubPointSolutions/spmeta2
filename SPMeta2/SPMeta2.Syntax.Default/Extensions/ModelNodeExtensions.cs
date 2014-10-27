using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default.Extensions
{
    public static class ModelNodeExtensions
    {
        #region methods

        public static ModelNode AddDefinitionNode(this ModelNode node, DefinitionBase definition)
        {
            return AddDefinitionNode(node, definition, null);
        }

        public static ModelNode AddDefinitionNode(this ModelNode node, DefinitionBase definition,
            Action<ModelNode> action)
        {
            return AddDefinitionNodeWithOptions(node, definition, action, null);
        }

        public static ModelNode AddDefinitionNodeWithOptions(this ModelNode node,
            DefinitionBase definition,
            Action<ModelNode> action, ModelNodeOptions options)
        {
            var modelNode = new ModelNode { Value = definition };

            // TODO, should be extend later
            if (options != null)
            {
                modelNode.Options = options;

                definition.RequireSelfProcessing = options.RequireSelfProcessing;
            }

            node.ChildModels.Add(modelNode);

            if (action != null)
                action(modelNode);

            return node;
        }

        #endregion
    }
}
