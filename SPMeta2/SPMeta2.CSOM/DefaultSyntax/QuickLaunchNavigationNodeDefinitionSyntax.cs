using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.DefaultSyntax
{
    public static class QuickLaunchNavigationNodeDefinitionSyntax
    {
        #region methods

        public static ModelNode OnCreating(this ModelNode model, Action<QuickLaunchNavigationNodeDefinition, NavigationNode> action)
        {
            model.RegisterModelEvent<QuickLaunchNavigationNodeDefinition, NavigationNode>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<QuickLaunchNavigationNodeDefinition, NavigationNode> action)
        {
            model.RegisterModelEvent<QuickLaunchNavigationNodeDefinition, NavigationNode>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
