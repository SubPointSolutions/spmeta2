using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Base
{
    public abstract class NavigationNodeModelHandler<TNavigationNode> : CSOMModelHandlerBase
      where TNavigationNode : NavigationNodeDefinitionBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // TMP
            if (modelHost is WebModelHost)
                CurrentClientContext = (modelHost as WebModelHost).HostClientContext;

            CurrentModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var quickLaunchNode = model.WithAssertAndCast<NavigationNodeDefinitionBase>("model", value => value.RequireNotNull());

            if (ShouldDeployRootNavigationNode(modelHost))
                EnsureRootNavigationNode(modelHost as WebModelHost, quickLaunchNode);
            else if (ShouldDeployNavigationNode(modelHost))
                EnsureNavigationNode(modelHost as NavigationNodeModelHost, quickLaunchNode);
            else
                throw new ArgumentException("modelHost needs to be WebModelHost or NavigationNodeModelHost");
        }

        protected bool ShouldDeployRootNavigationNode(object modelHost)
        {
            return modelHost is WebModelHost;
        }

        protected bool ShouldDeployNavigationNode(object modelHost)
        {
            return modelHost is NavigationNodeModelHost;
        }

        protected ClientContext CurrentClientContext { get; set; }
        protected CSOMModelHostBase CurrentModelHost { get; set; }

        protected NavigationNode LookupNodeForHost(object modelHost, NavigationNodeDefinitionBase definition)
        {
            if (modelHost is WebModelHost)
            {


                return LookupNavigationNode(GetNavigationNodeCollection((modelHost as WebModelHost).HostWeb), definition);
            }
            else if (modelHost is NavigationNodeModelHost)
                return LookupNavigationNode((modelHost as NavigationNodeModelHost).HostNavigationNode.Children, definition);
            else if (modelHost is NavigationNode)
                return LookupNavigationNode((modelHost as NavigationNode).Children, definition);

            throw new ArgumentException("modelHost needs to be SPWeb");
        }

        protected virtual NavigationNode LookupNavigationNode(NavigationNodeCollection nodes,
            NavigationNodeDefinitionBase definition)
        {
            var context = nodes.Context;

            context.Load(nodes);
            context.ExecuteQueryWithTrace();

            var currentNode = nodes
                                .OfType<NavigationNode>()
                                .FirstOrDefault(n => !string.IsNullOrEmpty(n.Title) && (n.Title.ToUpper() == definition.Title.ToUpper()));

            if (currentNode == null)
            {
                var url = ResolveTokenizedUrl(CurrentModelHost, definition);

                url = HttpUtility.UrlKeyValueDecode(url);

                currentNode = nodes
                                .OfType<NavigationNode>()
                                .FirstOrDefault(n => !string.IsNullOrEmpty(n.Url) && (n.Url.ToUpper().EndsWith(url.ToUpper())));
            }

            return currentNode;
        }

        protected NavigationNode GetNavigationNode(
            NavigationNodeModelHost navigationNodeModelHost,
            NavigationNodeDefinitionBase quickLaunchNode)
        {
            var navigationNode = navigationNodeModelHost.HostNavigationNode;
            var quickLaunch = navigationNode.Children;

            var context = navigationNodeModelHost.HostWeb.Context;

            context.Load(quickLaunch);
            context.ExecuteQueryWithTrace();

            var existingNode = LookupNavigationNode(quickLaunch, quickLaunchNode);

            return existingNode;
        }

        protected virtual string ResolveTokenizedUrl(CSOMModelHostBase context, NavigationNodeDefinitionBase rootNode)
        {
            return ResolveTokenizedUrl(context, rootNode.Url);
        }

        protected virtual string ResolveTokenizedUrl(CSOMModelHostBase context, string urlValue)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original Url: [{0}]", urlValue);

            var newUrlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
            {
                Value = urlValue,
                Context = context
            }).Value;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced Url: [{0}]", newUrlValue);

            return newUrlValue;
        }

        private NavigationNode EnsureNavigationNode(NavigationNodeModelHost navigationNodeModelHost,
            NavigationNodeDefinitionBase quickLaunchNode)
        {
            var navigationNode = navigationNodeModelHost.HostNavigationNode;
            var quickLaunch = navigationNode.Children;

            var context = navigationNodeModelHost.HostWeb.Context;

            context.Load(quickLaunch);
            context.ExecuteQueryWithTrace();

            var existingNode = LookupNavigationNode(quickLaunch, quickLaunchNode);

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
                    Url = ResolveTokenizedUrl(navigationNodeModelHost, quickLaunchNode),
                    AsLastNode = true
                });

                context.ExecuteQueryWithTrace();
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = ResolveTokenizedUrl(navigationNodeModelHost, quickLaunchNode);

#if !NET35
            existingNode.IsVisible = quickLaunchNode.IsVisible;
#endif

            ProcessLocalization(existingNode, quickLaunchNode);

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

            context.ExecuteQueryWithTrace();

            return existingNode;
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var quickLaunchNode = model as NavigationNodeDefinitionBase;

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
            NavigationNodeDefinitionBase quickLaunchModel)
        {
            NavigationNodeCollection quickLaunch = null;
            var result = GetRootNavigationNode(webModelHost, quickLaunchModel, out quickLaunch);

            return result;
        }

        protected abstract NavigationNodeCollection GetNavigationNodeCollection(Web web);



        protected NavigationNode GetRootNavigationNode(
            WebModelHost webModelHost,
            NavigationNodeDefinitionBase navigationNodeModel, out NavigationNodeCollection rootNavigationNodes)
        {
            var web = webModelHost.HostWeb;
            var context = webModelHost.HostWeb.Context;

            rootNavigationNodes = GetNavigationNodeCollection(web);

            context.Load(rootNavigationNodes);
            context.ExecuteQueryWithTrace();

            // TODO, crazy URL matching to find 'resolved URL'

            var url = ResolveTokenizedUrl(webModelHost, navigationNodeModel);

            var existingNode = rootNavigationNodes.OfType<NavigationNode>()
                .FirstOrDefault(n => n.Url.ToUpper().EndsWith(url.ToUpper()));

            return existingNode;
        }

        private NavigationNode EnsureRootNavigationNode(
            WebModelHost webModelHost,
            NavigationNodeDefinitionBase navigationNodeModel)
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new navigation node");

                existingNode = quickLaunch.Add(new NavigationNodeCreationInformation
                {
                    Title = navigationNodeModel.Title,
                    IsExternal = navigationNodeModel.IsExternal,
                    Url = ResolveTokenizedUrl(webModelHost, navigationNodeModel),
                    PreviousNode = previousNode
                });

                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing navigation node");
            }

            existingNode.Title = navigationNodeModel.Title;
            existingNode.Url = ResolveTokenizedUrl(webModelHost, navigationNodeModel);

#if !NET35
            existingNode.IsVisible = navigationNodeModel.IsVisible;
#endif

            ProcessLocalization(existingNode, navigationNodeModel);

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

            context.ExecuteQueryWithTrace();

            return existingNode;
        }


        protected virtual void ProcessLocalization(NavigationNode obj, NavigationNodeDefinitionBase definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "TitleResource", definition.TitleResource }
            });
        }

        #endregion
    }
}
