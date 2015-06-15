using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class QuickLaunchNavigationSyntax
    {
        public static ModelNode AddQuickLaunchNavigationNode(this ModelNode model, QuickLaunchNavigationNodeDefinition definition)
        {
            return AddQuickLaunchNavigationNode(model, definition, null);
        }

        public static ModelNode AddQuickLaunchNavigationNode(this ModelNode model, QuickLaunchNavigationNodeDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddQuickLaunchNavigationNodes(this ModelNode model, IEnumerable<QuickLaunchNavigationNodeDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
