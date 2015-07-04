using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class NavigationNodeModelNode : TypedModelNode
    {
    }

    public class QuickLaunchNavigationNodeModelNode : NavigationNodeModelNode
    {
        
    }

    // TODO
    public static class QuickLaunchNavigationSyntax
    {
        public static WebModelNode AddQuickLaunchNavigationNode(this WebModelNode model, QuickLaunchNavigationNodeDefinition definition)
        {
            return AddQuickLaunchNavigationNode(model, definition, null);
        }

        public static WebModelNode AddQuickLaunchNavigationNode(this WebModelNode model, QuickLaunchNavigationNodeDefinition definition,
            Action<QuickLaunchNavigationNodeModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static QuickLaunchNavigationNodeModelNode AddQuickLaunchNavigationNode(this QuickLaunchNavigationNodeModelNode model, QuickLaunchNavigationNodeDefinition definition)
        {
            return AddQuickLaunchNavigationNode(model, definition, null);
        }

        public static QuickLaunchNavigationNodeModelNode AddQuickLaunchNavigationNode(this QuickLaunchNavigationNodeModelNode model, QuickLaunchNavigationNodeDefinition definition,
            Action<QuickLaunchNavigationNodeModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
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
