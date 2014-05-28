using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class QuickLaunchNavigationNodeModelHandler : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(QuickLaunchNavigationNodeDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var quickLaunchNode = model as QuickLaunchNavigationNodeDefinition;

            if (modelHost is SPWeb)
                EnsureRootQuickLaunchNavigationNode(modelHost as SPWeb, quickLaunchNode);
            else if (modelHost is SPNavigationNode)
                EnsureQuickLaunchNavigationNode(modelHost as SPNavigationNode, quickLaunchNode);
            else
            {
                throw new ArgumentException("modelHost needs to be SPWeb");
            }
        }

        private SPNavigationNode EnsureQuickLaunchNavigationNode(SPNavigationNode navigationNode, QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var quickLaunch = navigationNode.Children;

            var existingNode = quickLaunch.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(quickLaunchNode.Title, quickLaunchNode.Url, quickLaunchNode.IsExternal);
                quickLaunch.AddAsLast(existingNode);
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = quickLaunchNode.Url;
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            existingNode.Update();

            return existingNode;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var quickLaunchNode = model as QuickLaunchNavigationNodeDefinition;

            if (modelHost is SPWeb)
            {
                var web = modelHost as SPWeb;
                var currentNode = EnsureRootQuickLaunchNavigationNode(web, quickLaunchNode);

                action(currentNode);
            }
            else if (modelHost is SPNavigationNode)
            {
                var node = modelHost as SPNavigationNode;
                var currentNode = EnsureQuickLaunchNavigationNode(node, quickLaunchNode);

                action(currentNode);
            }
            else
            {
                action(modelHost);
            }
        }

        private SPNavigationNode EnsureRootQuickLaunchNavigationNode(SPWeb web, QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var quickLaunch = web.Navigation.QuickLaunch;

            var existingNode = quickLaunch.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(quickLaunchNode.Title, quickLaunchNode.Url, quickLaunchNode.IsExternal);
                quickLaunch.AddAsLast(existingNode);
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = quickLaunchNode.Url;
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            existingNode.Update();

            return existingNode;
        }

        #endregion
    }
}
