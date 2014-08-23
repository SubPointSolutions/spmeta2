using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Common;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class QuickLaunchNavigationNodeModelHandler : CSOMModelHandlerBase
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
            var quickLaunchNode = model.WithAssertAndCast<QuickLaunchNavigationNodeDefinition>("model", value => value.RequireNotNull());

            NavigationNode node = null;

            InvokeOnModelEvent<QuickLaunchNavigationNodeDefinition, NavigationNode>(node, ModelEventType.OnUpdating);

            if (ShouldDeployRootNavigationNode(modelHost))
                node = EnsureRootQuickLaunchNavigationNode(modelHost as WebModelHost, quickLaunchNode);
            else if (ShouldDeployNavigationNode(modelHost))
                node = EnsureQuickLaunchNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchNode);
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

        private NavigationNode EnsureQuickLaunchNavigationNode(NavigationNodeModelHost navigationNodeModelHost,
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
                var currentNode = EnsureRootQuickLaunchNavigationNode(webModelHost, quickLaunchNode);

                var nodeHost = ModelHostBase.Inherit<NavigationNodeModelHost>(webModelHost, host =>
                {
                    host.HostNavigationNode = currentNode;
                });

                action(nodeHost);
            }
            else if (modelHost is NavigationNodeModelHost)
            {
                var nodeModelHost = modelHost as NavigationNodeModelHost;
                var currentNode = EnsureQuickLaunchNavigationNode(nodeModelHost, quickLaunchNode);

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

        protected NavigationNode GetRootNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchModel, out NavigationNodeCollection quickLaunch)
        {
            var web = webModelHost.HostWeb;
            var context = webModelHost.HostWeb.Context;

            quickLaunch = web.Navigation.QuickLaunch;

            context.Load(quickLaunch);
            context.ExecuteQuery();

            var existingNode = quickLaunch.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchModel.Url);

            return existingNode;
        }

        private NavigationNode EnsureRootQuickLaunchNavigationNode(
            WebModelHost webModelHost,
            QuickLaunchNavigationNodeDefinition quickLaunchModel)
        {
            NavigationNodeCollection quickLaunch = null;

            var context = webModelHost.HostWeb.Context;

            var existingNode = GetRootNavigationNode(webModelHost, quickLaunchModel, out quickLaunch);
            var previousNode = quickLaunch.Count > 0 ? quickLaunch.Last() : null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = quickLaunchModel,
                ModelHost = webModelHost
            });

            if (existingNode == null)
            {
                existingNode = quickLaunch.Add(new NavigationNodeCreationInformation
                {
                    Title = quickLaunchModel.Title,
                    IsExternal = quickLaunchModel.IsExternal,
                    Url = quickLaunchModel.Url,
                    PreviousNode = previousNode
                });

                context.ExecuteQuery();
            }

            existingNode.Title = quickLaunchModel.Title;
            existingNode.Url = quickLaunchModel.Url;
            existingNode.IsVisible = quickLaunchModel.IsVisible;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(NavigationNode),
                ObjectDefinition = quickLaunchModel,
                ModelHost = webModelHost
            });

            existingNode.Update();

            context.ExecuteQuery();

            return existingNode;
        }

        #endregion
    }
}
