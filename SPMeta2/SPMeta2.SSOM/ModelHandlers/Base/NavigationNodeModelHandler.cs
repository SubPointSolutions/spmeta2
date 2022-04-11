﻿using Microsoft.SharePoint.Navigation;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.SSOM.ModelHandlers.Base;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers.Base
{
    public abstract class NavigationNodeModelHandler<TNavigationNode> : SSOMModelHandlerBase
        where TNavigationNode : NavigationNodeDefinitionBase
    {

        #region properties

        protected WebModelHost CurrentWebModelHost { get; set; }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var navigationNode = model as TNavigationNode;

            // assuming deployment always goes from root web to te children
            // so that CurrentWebModelHost won't even be null

            if (modelHost is WebModelHost)
            {
                CurrentWebModelHost = modelHost as WebModelHost;
                EnsureRootNavigationNode(modelHost as WebModelHost, navigationNode as NavigationNodeDefinitionBase);
            }
            else if (modelHost is SPNavigationNode)
                EnsurehNavigationNode(modelHost as SPNavigationNode, navigationNode);
            else
            {
                throw new ArgumentException("modelHost needs to be SPWeb");
            }
        }

        protected SPNavigationNode LookupNodeForHost(object modelHost, NavigationNodeDefinitionBase definition)
        {
            if (modelHost is WebModelHost)
                return LookupNavigationNode(GetNavigationNodeCollection((modelHost as WebModelHost).HostWeb), definition);
            else if (modelHost is SPNavigationNode)
                return LookupNavigationNode((modelHost as SPNavigationNode).Children, definition);

            throw new ArgumentException("modelHost needs to be SPWeb");
        }

        private SPNavigationNode EnsurehNavigationNode(SPNavigationNode navigationNode, NavigationNodeDefinitionBase quickLaunchNode)
        {
            var topNavigationNode = navigationNode.Children;
            var existingNode = LookupNavigationNode(topNavigationNode, quickLaunchNode);

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
                existingNode = new SPNavigationNode(quickLaunchNode.Title, ResolveTokenizedUrl(CurrentWebModelHost, quickLaunchNode), quickLaunchNode.IsExternal);
                topNavigationNode.AddAsLast(existingNode);
            }

            existingNode.Title = quickLaunchNode.Title;
            existingNode.Url = ResolveTokenizedUrl(CurrentWebModelHost, quickLaunchNode);
            existingNode.IsVisible = quickLaunchNode.IsVisible;

            ProcessProperties(existingNode, quickLaunchNode);
            ProcessLocalization(existingNode, quickLaunchNode);

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

        protected virtual void ProcessProperties(SPNavigationNode existingNode, NavigationNodeDefinitionBase quickLaunchNode)
        {
            if (quickLaunchNode.Properties != null && quickLaunchNode.Properties.Any())
            {
                foreach (var prop in quickLaunchNode.Properties)
                {
                    if (existingNode.Properties.ContainsKey(prop.Key))
                        existingNode.Properties[prop.Key] = prop.Value;
                    else
                        existingNode.Properties.Add(prop.Key, prop.Value);
                }
            }
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
                CurrentWebModelHost = modelHost as WebModelHost;

                var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
                var currentNode = EnsureRootNavigationNode(webModelHost, quickLaunchNode);

                action(currentNode);
            }
            else if (modelHost is SPNavigationNode)
            {
                var node = modelHost as SPNavigationNode;
                var currentNode = EnsurehNavigationNode(node, quickLaunchNode);

                action(currentNode);
            }
            else
            {
                action(modelHost);
            }
        }

        protected abstract SPNavigationNodeCollection GetNavigationNodeCollection(SPWeb web);

        protected virtual SPNavigationNode LookupNavigationNode(SPNavigationNodeCollection nodes, NavigationNodeDefinitionBase definition)
        {
            var currentNode = nodes
                                .OfType<SPNavigationNode>()
                                .FirstOrDefault(n => !string.IsNullOrEmpty(n.Title) && (n.Title.ToUpper() == definition.Title.ToUpper()));

            if (currentNode == null)
            {
                var url = ResolveTokenizedUrl(CurrentWebModelHost, definition);

                url = HttpUtility.UrlDecode(url);

                currentNode = nodes
                                .OfType<SPNavigationNode>()
                                .FirstOrDefault(n => !string.IsNullOrEmpty(n.Url) && (n.Url.ToUpper().EndsWith(url.ToUpper())));
            }

            return currentNode;
        }

        protected virtual string ResolveTokenizedUrl(WebModelHost webModelHost, NavigationNodeDefinitionBase rootNode)
        {
            return ResolveTokenizedUrl(webModelHost, rootNode.Url);
        }

        protected virtual string ResolveTokenizedUrl(WebModelHost webModelHost, string tokenizedUrl)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original Url: [{0}]", tokenizedUrl);

            var newUrlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
            {
                Value = tokenizedUrl,
                Context = webModelHost.HostWeb
            }).Value;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced Url: [{0}]", newUrlValue);

            return newUrlValue;
        }

        private SPNavigationNode EnsureRootNavigationNode(WebModelHost webModelHost, NavigationNodeDefinitionBase rootNode)
        {
            var web = webModelHost.HostWeb;

            var quickLaunch = GetNavigationNodeCollection(web);
            var existingNode = LookupNavigationNode(quickLaunch, rootNode);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = rootNode,
                ModelHost = webModelHost
            });

            if (existingNode == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new navigation node");

                existingNode = new SPNavigationNode(rootNode.Title, ResolveTokenizedUrl(webModelHost, rootNode), rootNode.IsExternal);
                quickLaunch.AddAsLast(existingNode);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing navigation node");
            }

            existingNode.Title = rootNode.Title;
            existingNode.Url = ResolveTokenizedUrl(webModelHost, rootNode);
            existingNode.IsVisible = rootNode.IsVisible;

            ProcessProperties(existingNode, rootNode);
            ProcessLocalization(existingNode, rootNode);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingNode,
                ObjectType = typeof(SPNavigationNode),
                ObjectDefinition = rootNode,
                ModelHost = webModelHost
            });

            existingNode.Update();

            return existingNode;
        }


        protected virtual void ProcessLocalization(SPNavigationNode obj, NavigationNodeDefinitionBase definition)
        {
            if (definition.TitleResource.Any())
            {
                foreach (var locValue in definition.TitleResource)
                    LocalizationService.ProcessUserResource(obj, obj.TitleResource, locValue);
            }
        }

        #endregion
    }
}
