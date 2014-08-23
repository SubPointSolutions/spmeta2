using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class TopNavigationSyntax
    {
        public static ModelNode AddTopNavigationNode(this ModelNode model, TopNavigationNodeDefinition definition)
        {
            return AddTopNavigationNode(model, definition, null);
        }

        public static ModelNode AddTopNavigationNode(this ModelNode model, TopNavigationNodeDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
