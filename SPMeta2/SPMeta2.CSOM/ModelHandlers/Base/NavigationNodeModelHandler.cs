using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Common;
using SPMeta2.Utils;
using SPMeta2.Definitions.Base;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHandlers.Base
{
    public abstract class NavigationNodeModelHandler<TNavigationNode> : CSOMModelHandlerBase
      where TNavigationNode : NavigationNodeDefinitionBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var quickLaunchNode = model.WithAssertAndCast<QuickLaunchNavigationNodeDefinition>("model", value => value.RequireNotNull());

            NavigationNode node = null;

            InvokeOnModelEvent<QuickLaunchNavigationNodeDefinition, NavigationNode>(node, ModelEventType.OnUpdating);

            if (ShouldDeployRootNavigationNode(modelHost))
                node = EnsureRootNavigationNode(modelHost as WebModelHost, quickLaunchNode);
            else if (ShouldDeployNavigationNode(modelHost))
                node = EnsureNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchNode);
            else
                throw new ArgumentException("modelHost needs to be WebModelHost or NavigationNodeModelHost");

            InvokeOnModelEvent<QuickLaunchNavigationNodeDefinition, NavigationNode>(node, ModelEventType.OnUpdated);
        }

        protected bool ShouldDeployRootNavigationNode(object modelHost)
        {
            return modelHost is WebModelHost;
        }

        protected bool ShouldDeployNavigationNode(object modelHost)
        {
            return modelHost is NavigationNodeModelHost;
        }

        protected NavigationNode GetNavigationNode(
            NavigationNodeModelHost navigationNodeModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var navigationNode = navigationNodeModelHost.HostNavigationNode;
            var quickLaunch = navigationNode.Children;

            var context = navigationNodeModelHost.HostWeb.Context;

            context.Load(quickLaunch);
            context.ExecuteQuery();

            var existingNode = quickLaunch.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            return existingNode;
        }

        private NavigationNode EnsureNavigationNode(NavigationNodeModelHost navigationNodeModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var navigationNode = navigationNodeModelHost.HostNavigationNode;
            var quickLaunch = navigationNode.Children;

            var context = navigationNodeModelHost.HostWeb.Context;

            context.Load(quickLaunch);
            context.ExecuteQuery();

            var existingNode = GetNavigationNode(navigationNodeModelHost, quickLaunchNode);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = navigationNodeModelHost
            });

            if (existingNode == null)
            {
                existingNode = quickLaunch.Add(new NavigationNodeCreationInformation
                {
                    Title = quickLaunchNode.Title,
                    IsExternal = quickLaunchNode.IsExternal,
                    Url = quickLaunchNode.Url
                });

                context.ExecuteQuery();
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
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = quickLaunchNode,
                ModelHost = navigationNodeModelHost
            });

            existingNode.Update();

            context.ExecuteQuery();

            return existingNode;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var quickLaunchNode = model as QuickLaunchNavigationNodeDefinition;

            if (modelHost is WebModelHost)
            {
                var webModelHost = modelHost as WebModelHost;
                var currentNode = EnsureRootNavigationNode(webModelHost, quickLaunchNode);

                var nodeHost = ModelHostBase.Inherit<NavigationNodeModelHost>(webModelHost, host =>
                {
                    host.HostNavigationNode = currentNode;
                });

                action(nodeHost);
            }
            else if (modelHost is NavigationNodeModelHost)
            {
                var nodeModelHost = modelHost as NavigationNodeModelHost;
                var currentNode = EnsureNavigationNode(nodeModelHost, quickLaunchNode);

                var nodeHost = ModelHostBase.Inherit<NavigationNodeModelHost>(nodeModelHost, host =>
                {
                    host.HostNavigationNode = currentNode;
                });

                action(nodeHost);
            }
            else
            {
                action(modelHost);
            }
        }

        protected NavigationNode GetRootNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchModel)
        {
            NavigationNodeCollection quickLaunch = null;
            var result = GetRootNavigationNode(webModelHost, quickLaunchModel, out quickLaunch);

            return result;
        }

        protected abstract NavigationNodeCollection GetNavigationNodeCollection(Web web);



        protected NavigationNode GetRootNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition navigationNodeModel, out NavigationNodeCollection rootNavigationNodes)
        {
            var web = webModelHost.HostWeb;
            var context = webModelHost.HostWeb.Context;

            rootNavigationNodes = GetNavigationNodeCollection(web);

            context.Load(rootNavigationNodes);
            context.ExecuteQuery();

            var existingNode = rootNavigationNodes.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url == navigationNodeModel.Url);

            return existingNode;
        }

        private NavigationNode EnsureRootNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition navigationNodeModel)
        {
            NavigationNodeCollection quickLaunch = null;

            var context = webModelHost.HostWeb.Context;

            var existingNode = GetRootNavigationNode(webModelHost, navigationNodeModel, out quickLaunch);
            var previousNode = quickLaunch.Count > 0 ? quickLaunch.Last() : null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = navigationNodeModel,
                ModelHost = webModelHost
            });

            if (existingNode == null)
            {
                existingNode = quickLaunch.Add(new NavigationNodeCreationInformation
                {
                    Title = navigationNodeModel.Title,
                    IsExternal = navigationNodeModel.IsExternal,
                    Url = navigationNodeModel.Url,
                    PreviousNode = previousNode
                });

                context.ExecuteQuery();
            }

            existingNode.Title = navigationNodeModel.Title;
            existingNode.Url = navigationNodeModel.Url;
            existingNode.IsVisible = navigationNodeModel.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = navigationNodeModel,
                ModelHost = webModelHost
            });

            existingNode.Update();

            context.ExecuteQuery();

            return existingNode;
        }

        #endregion
    }
}
