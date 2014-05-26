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

namespace SPMeta2.CSOM.ModelHandlers
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

            NavigationNode node = null;

            InvokeOnModelEvents<QuickLaunchNavigationNodeDefinition, NavigationNode>(node, ModelEventType.OnUpdating);


            if (modelHost is WebModelHost)
                node = EnsureRootQuickLaunchNavigationNode(modelHost as WebModelHost, quickLaunchNode);
            else if (modelHost is NavigationNodeModelHost)
                node = EnsureQuickLaunchNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchNode);
            else
            {
                throw new ArgumentException("modelHost needs to be SPWeb");
            }

            InvokeOnModelEvents<QuickLaunchNavigationNodeDefinition, NavigationNode>(node, ModelEventType.OnUpdated);

        }

        private NavigationNode EnsureQuickLaunchNavigationNode(NavigationNodeModelHost navigationNodeModelHost, QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var navigationNode = navigationNodeModelHost.HostNavigationNode;
            var quickLaunch = navigationNode.Children;

            var context = navigationNodeModelHost.HostWeb.Context;

            context.Load(quickLaunch);
            context.ExecuteQuery();

            var existingNode = quickLaunch.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

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

        private NavigationNode EnsureRootQuickLaunchNavigationNode(WebModelHost webModelHost, QuickLaunchNavigationNodeDefinition quickLaunchNode)
        {
            var web = webModelHost.HostWeb;
            var context = webModelHost.HostWeb.Context;

            var quickLaunch = web.Navigation.QuickLaunch;

            context.Load(quickLaunch);
            context.ExecuteQuery();

            var existingNode = quickLaunch.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url == quickLaunchNode.Url);

            if (existingNode == null)
            {
                existingNode = quickLaunch.Add(new NavigationNodeCreationInformation
                {
                    Title = quickLaunchNode.Title,
                    IsExternal = quickLaunchNode.IsExternal,
                    Url = quickLaunchNode.Url,
                    PreviousNode = quickLaunch.Last()
                });

                context.ExecuteQuery();
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = quickLaunchNode.Url;
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            existingNode.Update();

            context.ExecuteQuery();

            return existingNode;
        }

        #endregion
    }
}
