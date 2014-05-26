using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class QuickLaunchNavigationSyntax
    {
        public static ModelNode AddQuickLaunchNavigationNode(this ModelNode model, DefinitionBase navigationNodeDefinition)
        {
            return AddQuickLaunchNavigationNode(model, navigationNodeDefinition, null);
        }

        public static ModelNode AddQuickLaunchNavigationNode(this ModelNode model, DefinitionBase navigationNodeDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = navigationNodeDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }
    }
}
