using Microsoft.SharePoint.Navigation;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TopNavigationNodeModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TopNavigationNodeDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var topNavigationNode = model as TopNavigationNodeDefinition;

            if (modelHost is WebModelHost)
                EnsureRootTopNavigationNode(modelHost as WebModelHost, topNavigationNode);
            else if (modelHost is SPNavigationNode)
                EnsureTopNavigationNode(modelHost as SPNavigationNode, topNavigationNode);
            else
            {
                throw new ArgumentException("modelHost needs to be SPWeb");
            }
        }

        private SPNavigationNode EnsureTopNavigationNode(
            SPNavigationNode navigationNode,
            TopNavigationNodeDefinition topNavigationNode)
        {
            var topNode = navigationNode.Children;

            var existingNode = topNode.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == topNavigationNode.Url);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = topNavigationNode,
                ModelHost = navigationNode
            });

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(topNavigationNode.Title, topNavigationNode.Url, topNavigationNode.IsExternal);
                topNode.AddAsLast(existingNode);
            }

            existingNode.Title = topNavigationNode.Title;
            existingNode.Url = topNavigationNode.Url;
            existingNode.IsVisible = topNavigationNode.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = topNavigationNode,
                ModelHost = navigationNode
            });

            existingNode.Update();

            return existingNode;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var topNode = model as TopNavigationNodeDefinition;

            if (modelHost is WebModelHost)
            {
                var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var currentNode = EnsureRootTopNavigationNode(webModelHost, topNode);

                action(currentNode);
            }
            else if (modelHost is SPNavigationNode)
            {
                var node = modelHost as SPNavigationNode;
                var currentNode = EnsureTopNavigationNode(node, topNode);

                action(currentNode);
            }
            else
            {
                action(modelHost);
            }
        }

        private SPNavigationNode EnsureRootTopNavigationNode(
            WebModelHost webModelHost,
            TopNavigationNodeDefinition topNavigationNode)
        {
            var web = webModelHost.HostWeb;

            var topNode = web.Navigation.TopNavigationBar;

            var existingNode = topNode.OfType<SPNavigationNode>()
                .FirstOrDefault(n => n.Url == topNavigationNode.Url);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = topNavigationNode,
                ModelHost = existingNode
            });

            if (existingNode == null)
            {
                existingNode = new SPNavigationNode(topNavigationNode.Title, topNavigationNode.Url, topNavigationNode.IsExternal);
                topNode.AddAsLast(existingNode);
            }

            existingNode.Title = topNavigationNode.Title;
            existingNode.Url = topNavigationNode.Url;
            existingNode.IsVisible = topNavigationNode.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = topNavigationNode,
                ModelHost = existingNode
            });

            existingNode.Update();

            return existingNode;
        }

        #endregion
    }
}
