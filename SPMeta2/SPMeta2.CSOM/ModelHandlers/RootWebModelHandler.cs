using System;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Services;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class RootWebModelHandler : CSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(RootWebDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(modelHost, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentObject,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(definition.Title))
                currentObject.Title = definition.Title;

            if (!string.IsNullOrEmpty(definition.Description))
                currentObject.Description = definition.Description;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentObject,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentObject.Update();
            currentObject.Context.ExecuteQueryWithTrace();
        }

        protected Web GetCurrentObject(object modelHost, RootWebDefinition definition)
        {
            if (modelHost is SiteModelHost)
            {
                var siteModelHost = modelHost as SiteModelHost;

                var site = siteModelHost.HostSite;
                var context = site.Context;

                context.Load(site, s => s.RootWeb);
                context.ExecuteQueryWithTrace();

                return site.RootWeb;
            }
            else if (modelHost is WebModelHost)
            {
                var webModelHost = modelHost as WebModelHost;

                var site = webModelHost.HostSite;
                var context = site.Context;

                context.Load(site, s => s.RootWeb);
                context.ExecuteQueryWithTrace();

                return site.RootWeb;
            }

            throw new SPMeta2UnsupportedModelHostException("ModelHost should be SiteModelHost/WebModelHost");
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(siteModelHost, definition);

            var rootModelHost = ModelHostBase.Inherit<WebModelHost>(siteModelHost, host =>
            {
                host.HostWeb = currentObject;
            });

            action(rootModelHost);

            currentObject.Update();
            currentObject.Context.ExecuteQueryWithTrace();
        }

        #endregion
    }
}
