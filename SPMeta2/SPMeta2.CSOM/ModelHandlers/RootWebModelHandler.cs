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
using SPMeta2.CSOM.Utils;
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
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(siteModelHost, definition);

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
            currentObject.Context.ExecuteQuery();
        }

        protected Web GetCurrentObject(SiteModelHost siteModelHost, RootWebDefinition definition)
        {
            var site = siteModelHost.HostSite;
            var context = site.Context;

            context.Load(site, s => s.RootWeb);
            context.ExecuteQuery();

            return site.RootWeb;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var currentObject = GetCurrentObject(siteModelHost, definition);

            var rootModelHost = ModelHostBase.Inherit<WebModelHost>(siteModelHost, host =>
            {
                host.HostWeb = currentObject;
            });

            action(rootModelHost);

            currentObject.Update();
            currentObject.Context.ExecuteQuery();
        }

        #endregion
    }
}
