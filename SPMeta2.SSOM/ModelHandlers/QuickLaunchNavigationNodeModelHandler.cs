using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class QuickLaunchNavigationNodeModelHandler : SSOMModelHandlerBase
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

            if (modelHost is WebModelHost)
                EnsureRootQuickLaunchNavigationNode(modelHost as WebModelHost, quickLaunchNode);
            else if (modelHost is SPNavigationNode)
                EnsureQuickLaunchNavigationNode(modelHost as SPNavigationNode, quickLaunchNode);
            else
            {
                throw new ArgumentException("modelHost needs to be SPWeb");
            }
        }

        private SPNavigationNode EnsureQuickLaunchNavigationNode(
            SPNavigationNode navigationNode,
            QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var quickLaunch = navigationNode.Children;

            var existingNode = quickLaunch.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = navigationNode
            });

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(quickLaunchNode.Title, quickLaunchNode.Url, quickLaunchNode.IsExternal);
                quickLaunch.AddAsLast(existingNode);
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = quickLaunchNode.Url;
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = navigationNode
            });

            existingNode.Update();

            return existingNode;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var quickLaunchNode = model as QuickLaunchNavigationNodeDefinition;

            if (modelHost is WebModelHost)
            {
                var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var currentNode = EnsureRootQuickLaunchNavigationNode(webModelHost, quickLaunchNode);

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

        private SPNavigationNode EnsureRootQuickLaunchNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var web = webModelHost.HostWeb;

            var quickLaunch = web.Navigation.QuickLaunch;

            var existingNode = quickLaunch.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = existingNode
            });

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(quickLaunchNode.Title, quickLaunchNode.Url, quickLaunchNode.IsExternal);
                quickLaunch.AddAsLast(existingNode);
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = quickLaunchNode.Url;
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = existingNode
            });

            existingNode.Update();

            return existingNode;
        }

        #endregion
    }
}
